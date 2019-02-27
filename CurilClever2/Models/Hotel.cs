using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurilClever2.Models
{
  public class Hotel
  {
    public int id { get; set; }
    public string Name { get; set; }
    public string Addres { get; set; }
    public int StarsRate { get; set; }
    public int LocationType { get; set; }
    public int Price { get; set; }
  }
}
