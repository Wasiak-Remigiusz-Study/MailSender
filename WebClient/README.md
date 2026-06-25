# WebClient – klient webowy dla MailSender API

## Opis projektu

Celem projektu było wygenerowanie klienta API na podstawie specyfikacji OpenAPI oraz stworzenie prostej aplikacji webowej umożliwiającej wysyłanie wiadomości e-mail za pomocą udostępnionego API MailSender.

Do komunikacji z API wykorzystano automatycznie wygenerowaną bibliotekę TypeScript, dzięki czemu nie było konieczności ręcznego tworzenia zapytań HTTP.

---

## Utworzenie projektu

Na początku utworzono projekt Node.js:

```bash
npm init -y
```

Polecenie wygenerowało plik `package.json`, zawierający konfigurację projektu oraz listę zależności.

---

## Instalacja OpenAPI Generator

Następnie zainstalowano narzędzie OpenAPI Generator:

```bash
npm install @openapitools/openapi-generator-cli --save-dev
```

Narzędzie służy do automatycznego generowania bibliotek klienckich na podstawie specyfikacji OpenAPI.

Podczas próby wygenerowania klienta wystąpił problem związany z brakiem środowiska Java (JDK), które jest wymagane przez pakiet `@openapitools/openapi-generator-cli`.

---

## Generowanie klienta TypeScript

W związku z problemem wykorzystano alternatywne narzędzie:

```bash
npm install openapi-typescript-codegen --save-dev
```

Następnie wygenerowano klienta TypeScript na podstawie pliku `openapi.json`:

```bash
npx openapi-typescript-codegen --input openapi.json --output generated-ts
```

W wyniku wygenerowano strukturę:

```text
generated-ts/
├── core/
├── models/
├── services/
│   ├── ClientAppService.ts
│   └── MailService.ts
└── index.ts
```

---

## Endpointy API

### Rejestracja aplikacji

```http
POST /client-app/register
```

Przykładowe dane:

```json
{
  "appId": "app1",
  "appName": "MailClient",
  "pass": "password"
}
```

### Wysyłanie wiadomości

```http
POST /mail/send
```

Przykładowe dane:

```json
{
  "to": "example@test.pl",
  "subject": "Test",
  "body": "Przykładowa wiadomość"
}
```

---

## Wykorzystanie wygenerowanego klienta

Do komunikacji z API wykorzystano wygenerowaną klasę `MailService`.

Przykładowe wywołanie:

```typescript
await MailService.postMailSend({
  to: "example@test.pl",
  subject: "Test",
  body: "Przykładowa wiadomość",
});
```

---

## Frontend

Aplikacja webowa składa się z:

- formularza HTML (`index.html`),
- pliku TypeScript (`main.ts`),
- wygenerowanego klienta API (`generated-ts`).

Użytkownik podaje:

- adres odbiorcy,
- temat wiadomości,
- treść wiadomości,

a następnie wysyła wiadomość przyciskiem **Wyślij**.

---

## TypeScript

Instalacja TypeScript:

```bash
npm install typescript --save-dev
```

Sprawdzenie wersji:

```bash
npx tsc --version
```

Kompilacja projektu:

```bash
npx tsc
```

---

## Uruchamianie aplikacji przy pomocy Vite

Do uruchomienia aplikacji wykorzystano Vite.

Instalacja:

```bash
npm install vite --save-dev
```

Uruchomienie serwera deweloperskiego:

```bash
npx vite
```

lub:

```bash
npm run dev
```

Po uruchomieniu aplikacja jest dostępna pod adresem:

```text
http://localhost:5173
```

Vite automatycznie kompiluje pliki TypeScript oraz odświeża stronę po każdej zmianie kodu.

---

## Struktura projektu

```text
WebClient/
├── generated-ts/
│   ├── core/
│   ├── models/
│   ├── services/
│   └── index.ts
│
├── index.html
├── styles.css
├── main.ts
├── openapi.json
├── package.json
├── package-lock.json
├── tsconfig.json
└── node_modules/
```

---

## Podsumowanie

W ramach projektu:

- wygenerowano klienta TypeScript na podstawie specyfikacji OpenAPI,
- utworzono prosty interfejs użytkownika w HTML,
- wykorzystano wygenerowaną bibliotekę do komunikacji z API,
- skonfigurowano środowisko TypeScript,
- uruchomiono aplikację przy użyciu Vite.

Projekt spełnia wymagania zadania dotyczące wykorzystania OpenAPI oraz stworzenia klienta webowego dla usługi MailSender.
