using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurilClever2.Models
{
  public class User
  {
    public int id { get; set; }
    public string name { get; set; }
    public string passwordHash { get; set; }
    public int accessLevel { get; set; }
  }
}
