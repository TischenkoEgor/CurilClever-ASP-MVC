using CurilClever2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurilClever2.ViewModels
{
  public class HomePageViewModel
  {
    public IEnumerable<Client> Clients;
    public IEnumerable<Hotel> Hotels;
    public IEnumerable<Order> Orders;
    public IEnumerable<Order> ActiveOrders;
    public IEnumerable<News> News;

    public int CountOfClients = 0;
    public int CountOfHotels = 0;
    public int CountOfOrders = 0;
    public int CountOfClientComments = 0;
    public int CountOfOrderComments = 0;
    
    public int MaxCount()
    {
      List<int> vals = new List<int>();
      vals.Add(CountOfClientComments);
      vals.Add(CountOfHotels);
      vals.Add(CountOfOrders);
      vals.Add(CountOfClientComments);
      vals.Add(CountOfOrderComments);

      return vals.Max();
    }

    public HomePageViewModel()
    {
      Clients = new List<Client>();
      Hotels = new List<Hotel>();
      Orders = new List<Order>();
      ActiveOrders = new List<Order>();
      News = new List<News>();
    }
  }
}
