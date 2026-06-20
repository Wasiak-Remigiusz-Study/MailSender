import { MailService } from "./generated-ts/services/MailService";
import { OpenAPI } from "./generated-ts/core/OpenAPI";

OpenAPI.BASE = "http://localhost:5110";

async function sendMail() {
  const token = (document.getElementById("token") as HTMLInputElement).value;
  OpenAPI.TOKEN = token;

  const to = (document.getElementById("to") as HTMLInputElement).value;
  const subject = (document.getElementById("subject") as HTMLInputElement).value;
  const body = (document.getElementById("body") as HTMLTextAreaElement).value;

  try {
    await MailService.postMailSend({
      to,
      subject,
      body,
    });
    alert("Wysłano pomyślnie!");
  } catch (error: any) {
    console.error("Mail send error:", error);
    const errorMsg = error.body?.error || error.message || "Wystąpił nieznany błąd";
    alert("Błąd podczas wysyłania: " + errorMsg);
  }
}

(window as any).sendMail = sendMail;
