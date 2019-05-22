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
      // пробуем
      try
      {
        //создаем новое письмо
        MailMessage mail = new MailMessage();
        // заполняем адрес от кого
        mail.From = new MailAddress(from);
        // заполняем адрес кому
        mail.To.Add(new MailAddress(email));
        // заполняем заголовок
        mail.Subject = subject;
        // заполняем сообщение
        mail.Body = text;
        // обозначаем что текст письма содержит HTML код
        mail.IsBodyHtml = true;

        // Создаем новый почтовый SMTP клиент
        SmtpClient client = new SmtpClient(smtpServer, port);
        // включаем SSL защиту соединения
        client.EnableSsl = enableSsl;
        // записываем логин и пароль от используемого для рассылки почтового аккаунта
        client.Credentials = new NetworkCredential(login, password);
        // записываем стандартный способ доставки (через сетевое соединение)
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        // отправляем письмо с помощью почтового клиента
        client.Send(mail);
        //удаляем письмо
        mail.Dispose();
        //сохраняем статус что все ок 
        status = "ok";

        return true;
      }

      catch (Exception e)
      {
        // если произошла ошибка при отправке, сохраняем текст ошибки для отладки
        status = "Error: " + e.Message + " \n" + e.InnerException != null ? e.InnerException.Message : "";
        return false;
      }
    }

    /// <summary>
    /// функция для отправки уведомлений всем подписанным пользователям
    /// </summary>
    /// <param name="news">новость для отправки</param>
    /// <param name="subs">лист подписок</param>
    /// <returns></returns>
    public static bool SendNewsToSubscribers(News news, List<Subscribe> subs)
    {
      // перебираем все подписки
      foreach(Subscribe sub in subs)
      {
        //если у подписки отправка новостей отключена, пропускаем ее
        if (!sub.SendNews) continue;
        // для сохранения результата отправки
        string result;
        // отправляем новость на отправку на почту
        SendTo(news.TextFull, news.TextShort, sub.User.Login, out result);
      }
      return true;
    }
  }
}
