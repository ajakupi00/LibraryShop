using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace PRA.Controllers
{
    public class LibraryController : Controller
    {
        // GET: Library
        public ActionResult Contact(string fname, string email, string title, string message)
        {
            SendMail2Step("smtp.gmail.com", 587, "aapps333@gmail.com",
                                "tkpuatpsqjeoypjo",
                                "aapps333@gmail.com", fname + ": "+title, message, null);
            return RedirectToAction("Index", "Home");
        }

        public void SendMail2Step(string SMTPServer, int SMTP_Port, string From, string Password, string To, string Subject, string Body, string[] FileNames)
        {
            var smtpClient = new SmtpClient(SMTPServer, SMTP_Port)
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = true
            };
            smtpClient.Credentials = new NetworkCredential(From, Password); //Use the new password, generated from google!

            var message = new MailMessage
            {
                From = new MailAddress(From, "Library d.o.o"),
                Subject = Subject,
                Body = Body,
            };
            message.To.Add(new MailAddress(To, To));
            smtpClient.Send(message);
        }
    }
}