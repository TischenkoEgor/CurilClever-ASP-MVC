using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurilClever2.Models;
using CurilClever2.Utils;
using Microsoft.AspNetCore.Mvc;

namespace CurilClever2.Controllers
{
  public class TestController : Controller
  {
    private CleverDBContext db;

    public TestController(CleverDBContext db)
    {
      this.db = db;
    }

    public IActionResult Index()
    {
      return View();
    }

    public IActionResult AddRandomClients()
    {
      string firstNamesMalestr = "Артём Александр Максим Сергей Кирилл Михаил Егор Матвей Денис Марат Владимир Глеб Константин Евгений Николай Фёдор Артур Виктор Олег Пётр Юра Виталий Василий Тихон Борис Валентин";
      string secondNamesMalestr = "Ильин Максимов Поляков Сорокин Виноградов Ковалев Белов	Медведев Антонов Тарасов Жуков Баранов Филиппов Комаров	Давыдов Беляев Герасимов Богданов Осипов Сидоров Матвеев Титов Марков Миронов Крылов Куликов Карпов Власов Мельников Денисов Гаврилов Тихонов Казаков Афанасьев Данилов Савельев Тимофеев Фомин Чернов Абрамов Мартынов Ефимов Федотов";
      string firstNamesFemalestr = "Анастасия Дарья Мария Анна Виктория Полина Елизавета Екатерина Ксения Валерия Варвара Александра Вероника Арина Алиса Алина Милана Маргарита Диана Ульяна Алёна Ангелина Кристина Юлия Кира Ева Карина Василиса Ольга Татьяна Ирина Таисия Евгения Яна Вера Марина Елена Надежда Светлана Злата Олеся Наталья Эвелина Лилия Элина Виолетта Нелли Мирослава Любовь Альбина Владислава Камилла Ника Ярослава";
      string secondNamesFemalestr = "Орлова Лебедева Симонова Александрова Третьякова Ленская Каменских Кожевникова Денисова Андреева Толмачева Шевченко Панченко Назарова Безрукова Соколова Родочинская Волкова Ковалевская Обломова Королева Волочкова Матвеева Левченко Лионова Котова Братиславская Полякова Ефимова Малышева Тарасова Новицкая Новикова Истомина Ивлева Ульянова Романова Гронская Бондаренко Хованская";
      


      List<string> firstNamesMale = firstNamesMalestr.Split(new char[1] {' '}).ToList();
      List<string> secondNamesMale = secondNamesMalestr.Split(new char[1] { ' ' }).ToList();
      List<string> firstNamesFemale = firstNamesFemalestr.Split(new char[1] { ' ' }).ToList();
      List<string> secondNamesFemale = secondNamesFemalestr.Split(new char[1] { ' ' }).ToList();

      Random rnd = new Random(Environment.TickCount);
      for (int i = 0; i < 100; i++)
      {
        int year = rnd.Next(1946, 2012);
        DateTime startDate = new DateTime(1946, 1, 1);
        DateTime endDate = new DateTime(2012, 12, 31);
        int range = (endDate - startDate).Days;
        DateTime BirthDate = startDate.AddDays(rnd.Next(range));

        Client client = new Client();
        client.Gender = rnd.Next() % 2 == 0 ? Gender.Male : Gender.Female;

        if(client.Gender == Gender.Male)
        {
          client.FirstName = RandHelper<string>.RandomItem(firstNamesMale, rnd);
          client.SecondName = RandHelper<string>.RandomItem(secondNamesMale, rnd);
        }
        else
        {
          client.FirstName = RandHelper<string>.RandomItem(firstNamesFemale, rnd);
          client.SecondName = RandHelper<string>.RandomItem(secondNamesFemale, rnd);
        }

        client.Phone = RandHelper<string>.randomPhone(rnd);
        client.Email = RandHelper<string>.randomEmail(rnd, client.FirstName, client.SecondName);
        client.PassportData = RandHelper<string>.randomPassoprt(rnd);
        client.DateOfBirthday = BirthDate;

        db.Clients.Add(client);
      }
      db.SaveChanges();
      return RedirectToAction("index");
    }


  }
}