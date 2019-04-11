using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurilClever2.Models
{
  public class ClientComment
  {
    public int id { get; set; }
    
    public int Userid { get; set; }
    public User User { get; set; }

    public string Text { get; set; }
    public DateTime Posted { get; set; }

    public int Clientid { get; set; }
    public Client Client { get; set; }
  }
}
