using CurilClever2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurilClever2.ViewModels
{
  public class HotelPageViewModel
  {
    public IEnumerable<Hotel> Hotels { get; set; }
    public PageViewModel PageViewModel { get; set; }
  }
}
