import { MailService } from "./generated-ts/services/MailService";
import { OpenAPI } from "./generated-ts/core/OpenAPI";

OpenAPI.BASE = "http://localhost:5110";
OpenAPI.TOKEN = import.meta.env.VITE_API_TOKEN;

async function sendMail() {
  const token = (document.getElementById("token") as HTMLInputElement).value;
  OpenAPI.TOKEN = token;

  const to = (document.getElementById("to") as HTMLInputElement).value;
  const subject = (document.getElementById("subject") as HTMLInputElement)
    .value;
  const body = (document.getElementById("body") as HTMLTextAreaElement).value;

  await MailService.postMailSend({
    to,
    subject,
    body,
  });

  alert("Wysłano");
}

(window as any).sendMail = sendMail;
