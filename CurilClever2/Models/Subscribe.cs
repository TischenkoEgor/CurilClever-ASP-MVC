using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurilClever2.Models
{
  public class Subscribe
  {
    public int id { get; set; }

    public int Userid { get; set; }
    public User User { get; set; }
    public bool SendNews { get; set; }
  }
}
