using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurilClever2.Models
{
  public class OrderComment
  {
    public int id { get; set; }

    public int Userid { get; set; }
    public User User { get; set; }

    public string Text { get; set; }
    public DateTime Posted { get; set; }

    public int Orderid { get; set; }
    public Order Order { get; set; }
  }
}
