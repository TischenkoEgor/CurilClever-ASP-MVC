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
    public HomePageViewModel()
    {
      Clients = new List<Client>();
      Hotels = new List<Hotel>();
      Orders = new List<Order>();
      ActiveOrders = new List<Order>();
    }

  }
}
