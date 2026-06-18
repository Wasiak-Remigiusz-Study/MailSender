import { MailService } from "./generated-ts/services/MailService";
import { OpenAPI } from "./generated-ts/core/OpenAPI";

OpenAPI.BASE = "https://localhost:5110";

async function sendMail() {
  const token = document.getElementById("token").value;
  const to = document.getElementById("to").value;
  const subject = document.getElementById("subject").value;
  const body = document.getElementById("body").value;

  await MailService.postMailSend({
    token,
    to,
    subject,
    body,
  });

  alert("Wysłano");
}

window.sendMail = sendMail;
