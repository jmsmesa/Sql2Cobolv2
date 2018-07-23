using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace cMail
{
    public class Mail
    {
        public string Remitente { get; set; }
        public List<string> Destinatario { get; set; }
        public List<string> ConCopia { get; set; }
        public List<string> ConCopiaOculta { get; set; }
        public string Asunto { get; set; }
        public string Cuerpo { get; set; }
        public List<string> Archivo { get; set; }
        public string MensajeError { get; set; }

        public bool EnivarCorreo()
        {
            cFunciones.AppSetting fn = new cFunciones.AppSetting();
            MailMessage mail = new MailMessage();
            Encoding enc = Encoding.GetEncoding(1253);
            Encoding StreamEncoding = Encoding.GetEncoding("iso-8859-1");

            string Host = fn.ObtenerAppSettings("host");
            Int32 Puerto = Convert.ToInt32(fn.ObtenerAppSettings("puerto"));
            string Sender = fn.ObtenerAppSettings("sender");
            string Passwdord = fn.ObtenerAppSettings("password");
            bool Ssl = fn.ObtenerAppSettings("ssl") == "S";

            try
            {
                mail.From = new MailAddress(Remitente);

                for (Int16 ind = 0; ind < Destinatario.Count; ind++)
                    mail.To.Add(new MailAddress(Destinatario[ind]));

                for (Int16 ind = 0; ind < ConCopia.Count; ind++)
                    mail.CC.Add(new MailAddress(ConCopia[ind]));

                for (Int16 ind = 0; ind < ConCopiaOculta.Count; ind++)
                    mail.Bcc.Add(new MailAddress(ConCopiaOculta[ind]));

                for (Int16 ind = 0; ind < Archivo.Count; ind++)
                    mail.Attachments.Add(new Attachment(Archivo[ind]));

                mail.Subject = Asunto;
                mail.IsBodyHtml = true;
                mail.Body = Cuerpo.Replace(Environment.NewLine, "<br />");
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient(Host)
                {
                    EnableSsl = Ssl,
                    Port = Puerto,
                    UseDefaultCredentials = true,
                    Credentials = new NetworkCredential(Sender, Passwdord)
                };
                smtp.Send(mail);

                MensajeError = string.Empty;
                mail.Dispose();
                return true;
            }
            catch (SmtpException ex)
            {
                mail.Dispose();
                MensajeError = ex.Message;
                return false;
            }
        }
    }
}