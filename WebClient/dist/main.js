import { MailService } from "./generated-ts/services/MailService";
import { OpenAPI } from "./generated-ts/core/OpenAPI";
OpenAPI.BASE = "https://localhost:5110";
async function sendMail() {
    const to = document.getElementById("to").value;
    const subject = document.getElementById("subject")
        .value;
    const body = document.getElementById("body").value;
    await MailService.postMailSend({
        to,
        subject,
        body,
    });
    alert("Wysłano");
}
window.sendMail = sendMail;
