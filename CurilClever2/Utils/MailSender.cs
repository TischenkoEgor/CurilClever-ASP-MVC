using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using CurilClever2.Models;
namespace CurilClever2.Utils
{
  public class MailSender
  {
    static string smtpServer = "smtp.gmail.com";
    static string login = "noreply.stat.mega@gmail.com";
    static int port = 587;
    static bool enableSsl = true;
    static string from = "noreply.stat.mega@gmail.com";
    static string password = "135792468Me";
    public static bool SendTo(string text, string subject, string email, out string status)
    {
      try
      {

        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(from);
        mail.To.Add(new MailAddress(email));
        mail.Subject = subject;
        mail.Body = text;
        mail.IsBodyHtml = true;

        SmtpClient client = new SmtpClient(smtpServer, port);
        //                client.Host = smtpServer;
        //                client.Port = port;
        client.EnableSsl = enableSsl;
        client.Credentials = new NetworkCredential(login, password);
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        client.Send(mail);
        mail.Dispose();
        status = "ok";
        return true;
      }

      catch (Exception e)
      {
        status = "Error: " + e.Message + " \n" + e.InnerException != null ? e.InnerException.Message : "";
        return false;
      }
    }

    
    public static bool SendNewsToSubscribers(News news, List<Subscribe> subs)
    {
      foreach(Subscribe sub in subs)
      {
        if (!sub.SendNews) continue;
        string result;
        SendTo(news.TextFull, news.TextShort, sub.User.Login, out result);
      }
      return false;
    }
  }
}
