using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurilClever2.Models
{
  public class Order
  {
    public int id { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime BeginTravelDate { get; set; }
    public DateTime EndTravelDate { get; set; }

    public int HoteId { get; set; }
    public Hotel Hotel { get; set; }

    public int ClientId { get; set; }
    public Client Client { get; set; }
 
    int Price { get; set; }
    int PayStatus { get; set; }
  }
}
