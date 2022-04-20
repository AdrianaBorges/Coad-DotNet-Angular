using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.LEGADO.Service;
using COAD.CORPORATIVO.Model.Dto;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Service;
using GenericCrud.Service;
using GenericCrud.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.CORPORATIVO.Config;
using COAD.CORPORATIVO.Model;

using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using System.ComponentModel;
using OpenPop.Mime;

namespace COAD.CORPORATIVO.Service.Custons
{
    public class CustomEmailSRV
    {

        EmailConfig emailConfig = new EmailConfig();
        static bool mailSent = false;

        public List<Email> LerMails()
        {

            try
            {

                using (var client = new OpenPop.Pop3.Pop3Client())
                {

                    client.Connect(emailConfig._hostname, emailConfig._port, emailConfig._useSsl);
                    client.Authenticate(emailConfig._username, emailConfig._password);
                    int messageCount = client.GetMessageCount();
                    emailConfig._emails.Clear();

                    int low = 0;
                    
                    if (messageCount > 10) low = messageCount - 10;

                    for (int i = messageCount; i > low; i--)
                    {

                        Message popEmail = client.GetMessage(i);
                        var popText = popEmail.FindFirstPlainTextVersion();
                        var popHtml = popEmail.FindFirstHtmlVersion();

                        String mailText = string.Empty;
                        String mailHtml = string.Empty;
                        if (popText != null)
                            mailText = popText.GetBodyAsText();
                        if (popHtml != null)
                            mailHtml = popHtml.GetBodyAsText();

                        List<MessagePart> attachs = popEmail.FindAllAttachments();

                        for (int contador = 0; contador < attachs.Count(); contador++)
                        {

                            string textoEm64 = System.Convert.ToBase64String(attachs[contador].Body);

                            string aSerSubstituida = "cid:" + attachs[contador].ContentId + "";
                            string aSubstituta = "data:" + attachs[contador].ContentType.MediaType + ";base64," + textoEm64 + "";

                            int inicio = mailHtml.IndexOf(aSerSubstituida);
                            int tamanho = aSerSubstituida.Length;

                            if ( inicio > 0 ) mailHtml = mailHtml.Substring(0, inicio) + aSubstituta + mailHtml.Substring(inicio + tamanho);

                        }

                        emailConfig._emails.Add(new Email()
                        {

                            Id = popEmail.Headers.MessageId,
                            Assunto = popEmail.Headers.Subject,
                            De = popEmail.Headers.From.Address,
                            Para = string.Join("; ", popEmail.Headers.To),
                            Data = popEmail.Headers.DateSent,
                            ConteudoTexto = mailText,
                            ConteudoHtml = !string.IsNullOrWhiteSpace(mailHtml) ? mailHtml : mailText

                        });

                    }

                }

            }

            catch (Exception e)
            {

                throw new Exception("Ocorreu um erro ao listar os e-mails", e);

            }

            return emailConfig._emails;

        }

        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                Console.WriteLine("[{0}] Send canceled.", token);
            }
            if (e.Error != null)
            {
                Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                Console.WriteLine("Message sent.");
            }
            mailSent = true;
        }

        public void EnviarEmail (Email email)
        {

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);

            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(emailConfig._username.Split(':').Last(), emailConfig._password);

            MailAddress from = new MailAddress(email.De, "COAD - Soluções confiáveis", System.Text.Encoding.UTF8);
            // Set destinations for the email message.
            //MailAddress to;
            
                
             //= new MailAddress(email.Para);
            // Specify the message content.
            MailMessage message = new MailMessage();
            message.From = from;

            foreach (String para in email.Para.Split(';'))
                message.To.Add(para);

            message.Body = email.ConteudoHtml;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Subject = email.Assunto;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            // Set the method that is called back when the send operation ends.
            //client.SendCompleted += new
            //SendCompletedEventHandler(SendCompletedCallback);
            // The userState can be any object that allows your callback 
            // method to identify this send operation.
            // For this example, the userToken is a string constant.
            //string userState = "test message1";
            /*
            client.SendAsync(message, "Enviando email");
            Console.WriteLine("Sending message... press c to cancel mail. Press any other key to exit.");
            string answer = Console.ReadLine();
            // If the user canceled the send, and mail hasn't been sent yet,
            // then cancel the pending operation.
            if (answer.StartsWith("c") && mailSent == false)
            {

                client.SendAsyncCancel();
                message.Dispose();
                return false;

            }
            else
            {

                message.Dispose();
                return true;

            }
            */

            client.Send(message);

        }

    }


}

