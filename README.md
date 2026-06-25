# MailSender

Projekt backendu zaimplementowany w .NET. Pozwala na autoryzację tokenami JWT oraz wysyłanie e-maili przy wykorzystaniu zewnętrznych providerów.

## Zadanie 3: Clean Architecture & Providers
W zadaniu 3 aplikacja została zrefaktoryzowana do modelu warstwowego (Clean Architecture):
- **Core**: Interfejsy (`IMailSenderProvider`) oraz Encje domenowe.
- **Infrastructure**: Komunikacja ze światem zewnętrznym. Posiada implementacje providerów (np. `BrevoMailSender`, `MailTrapMailSender`) oraz kontekst bazy danych.
- **Application**: Usługi wyższej warstwy (`MailService`, `ClientAppService`), które zawierają czystą logikę biznesową bez obaw o detale infrastrukturalne.

Zastosowano tu wzorzec Dependency Injection do dynamicznej podmiany sposobu wysyłki e-maili.

### Jak podmienić dostawcę e-mail?
Domyślnym dostawcą w aplikacji jest `BrevoMailSender`. Aby przełączyć aplikację na korzystanie z MailTrap, otwórz plik `Program.cs` i podmień rejestrację w kontenerze DI (Dependency Injection) poprzez zakomentowanie Brevo i odkomentowanie MailTrap:
```csharp
// builder.Services.AddTransient<IMailSenderProvider, BrevoMailSender>();
// mailtrap provider
builder.Services.AddTransient<IMailSenderProvider, MailTrapMailSender>();
```
Po ponownym uruchomieniu aplikacji wszystkie wysyłane maile będą z MailTrap.

## Zadanie 4: Logi i Baza Danych (InMemory)
Zadanie 4 wprowadza obsługę bazy danych Entity Framework Core (In-Memory).
- Rejestracja nowej aplikacji klienta jest sprawdzana pod kątem unikalności (`AppId` i `AppName`). Duplikaty otrzymują status `409 Conflict`. Wymuszony jest rygorystyczny format hasła: `q##waQ{dwie_ostatnie_cyfry_indeksu}`.
- Każde użycie endpointu `/mail/send` kończy się zapisaniem logu do bazy danych, włączając w to dane adresata, temat, status "powodzenie" lub "błąd", a także informacje identyfikujące aplikację (AppId oraz AppName).
- Utworzono nowe autoryzowane endpointy `/mail-log` oraz `/mail-log/{id}`, które filtrują i wyświetlają tylko logi wygenerowane przez aplikację klienta posiadającą dany Token JWT.
