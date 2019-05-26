using CurilClever2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CurilClever2.ViewModels
{
  public class AdminHomePageViewModel
  {
    [Display(Name ="посещений главной страницы")]
    public int CountHomePageVisits { get; set; }
    [Display(Name = "посещений страницы управления клиентами")]
    public int CountManageClientsPageVisits { get; set; }
    [Display(Name = "посещений страницы управления отелями")]
    public int CountManageHotelpageVisits { get; set; }
    [Display(Name = "посещений страницы управления заявками")]
    public int CountManageOrderPageVisits { get; set; }
    public Dictionary<Hotel, int> HotelVisits { get; set; }
    public Dictionary<Client, int> ClientVisits { get; set; }
    public Dictionary<Order, int> OrderVisits { get; set; }

    public AdminHomePageViewModel()
    {
      HotelVisits = new Dictionary<Hotel, int>();
      ClientVisits = new Dictionary<Client, int>();
      OrderVisits = new Dictionary<Order, int>();
    }

    public void BuildStat(CleverDBContext db)
    {
      // считаем посещения домашней страницы по количеству записей в базе, где путь состоит из строки "/"
      CountHomePageVisits = db.Visits.Count(v => v.path.ToLower() == "/");
      CountManageClientsPageVisits = db.Visits.Count(v => v.path.ToLower() == "/client");
      CountManageHotelpageVisits = db.Visits.Count(v => v.path.ToLower() == "/hotel");
      CountManageOrderPageVisits = db.Visits.Count(hui => hui.path.ToLower() == "/order");

      foreach(Visit vizit in db.Visits.Where(v => v.path.ToLower().Contains("/client/details/")))
      {
        // получаем id отрезая часть ссылки до цифр
        int id = int.Parse(vizit.path.ToLower().Replace("/client/details/", ""));
        // получаем клиента из базы
        Client client = db.Clients.Find(id);
        // если клиент в базе не обнаружен: пропускам этот визит
        if (client == null) continue;
        // если такого клиента нет в словаре посещений, добавляем клиента в словарь
        if (!ClientVisits.ContainsKey(client))
          ClientVisits.Add(client, 0);
        // увеличиваем количество посещений для этого клиента 
        ClientVisits[client]++;
      }
      foreach (Visit vizit in db.Visits.Where(v => v.path.ToLower().Contains("/hotel/details/")))
      {
        // получаем id отрезая часть ссылки до цифр
        int id = int.Parse(vizit.path.ToLower().Replace("/hotel/details/", ""));
        // получаем отель из базы
        Hotel hotel = db.Hotels.Find(id);
        // если отель в базе не обнаружен: пропускам этот визит
        if (hotel == null) continue;
        // если такого отеля нет в словаре посещений, добавляем клиента в словарь
        if (!HotelVisits.ContainsKey(hotel))
          HotelVisits.Add(hotel, 0);
        // увеличиваем количество посещений для этого отеля 
        HotelVisits[hotel]++;
      }
      foreach (Visit vizit in db.Visits.Where(v => v.path.ToLower().Contains("/order/details/")))
      {
        // получаем id отрезая часть ссылки до цифр
        int id = int.Parse(vizit.path.ToLower().Replace("/order/details/", ""));
        // получаем заявку из базы
        Order order = db.Orders.Find(id);
        // если заявки в базе не обнаружен: пропускам этот визит
        if (order == null) continue;
        // если такого клиента нет в словаре посещений, добавляем клиента в словарь
        if (!OrderVisits.ContainsKey(order))
          OrderVisits.Add(order, 0);
        // увеличиваем количество посещений для этого клиента 
        OrderVisits[order]++;
      }

    }
  }
}
