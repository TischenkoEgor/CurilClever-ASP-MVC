using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurilClever2.Models
{
  public class Client
  {
    public int id { get; set; }
    public string FirstName { get; set; }
    public string SecondName { get; set; }
    public int Age { get; set; }
    public int Sex { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string PassportData { get; set; }
  }
}
