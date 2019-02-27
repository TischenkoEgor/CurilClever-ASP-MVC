using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurilClever2.Models
{
  public class ClientComment
  {
    public int id { get; set; }

    public int CommetId { get; set; }
    public Comment Comment { get; set; }

    public int ClientId { get; set; }
    public Client Client { get; set; }
  }
}
