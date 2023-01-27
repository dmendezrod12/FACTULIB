using System;
using System.Net;
using System.Net.Mail;

namespace FactuLib.Library
{
    public class LSendMails
    {
        string urlDomain = "https://localhost:44325/";
        
        public void SendEmail(string email, string token)
        {
            string EmailOrigen = "factulibdieka@hotmail.com";
            string EmailDestino = email;
            string Contraseña = "LibDieka2023";
            string url = urlDomain +"Home/Recovery?token="+token;

            MailMessage oMailMessage = new MailMessage(EmailOrigen, EmailDestino, "Recuperación de contraseña FactuLib", 
                "<p> Correo para Recuperación de contraseña </p><br>" +
                "<a href='"+url+"'> Click para recuperar </a>");

            oMailMessage.IsBodyHtml= true;

            SmtpClient oSmtpClient = new SmtpClient("smtp.office365.com.");
            oSmtpClient.EnableSsl= true;
            oSmtpClient.UseDefaultCredentials= false;
            oSmtpClient.Port = 587;
            oSmtpClient.Credentials = new NetworkCredential(EmailOrigen, Contraseña);
            oSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            oSmtpClient.Send(oMailMessage);
        }
    }
}
