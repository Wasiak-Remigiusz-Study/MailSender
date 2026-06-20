using MailSender.Application.Services;
using MailSender.Application.Interfaces;
using MailSender.Infrastructure.Providers;
using MailSender.Infrastructure.Data;
using MailSender.WebApi.Services;
using MailSender.WebApi.Services.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
using Microsoft.EntityFrameworkCore;

var currentDir = new DirectoryInfo(AppContext.BaseDirectory);
while (currentDir != null && !File.Exists(Path.Combine(currentDir.FullName, ".env")))
{
    currentDir = currentDir.Parent;
}
if (currentDir != null)
{
    DotNetEnv.Env.Load(Path.Combine(currentDir.FullName, ".env"));
}
else
{
    DotNetEnv.Env.Load();
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("MailSenderDb"));

builder.Services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173", "http://localhost:5174")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Paste token."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("bearer", document)] = []
    });
});

var jwtSection = builder.Configuration.GetSection(JwtSettings.SectionName);
var jwtConfig = jwtSection.Get<JwtSettings>() ?? throw new InvalidOperationException("Jwt settings are missing.");
builder.Services.AddJwtTokenService(jwtSection);

builder.Services.AddScoped<ClientAppService>();
builder.Services.AddScoped<MailService>();
builder.Services.AddHttpClient<BrevoMailSender>();
builder.Services.AddHttpClient<MailTrapMailSender>();
builder.Services.AddTransient<IMailSenderProvider, BrevoMailSender>();
// mailtrap provider
//builder.Services.AddTransient<IMailSenderProvider, MailTrapMailSender>();

builder.Services
.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfig.Issuer,
        ValidAudience = jwtConfig.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key))
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
