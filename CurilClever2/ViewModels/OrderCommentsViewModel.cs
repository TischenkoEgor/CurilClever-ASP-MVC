using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurilClever2.Models;
namespace CurilClever2.ViewModels
{
  public class OrderCommentsViewModel
  {
    public int orderid { get; set;}
    public IEnumerable<OrderComment> Comments { get; set; }
    public PageViewModel PageViewModel { get; set; }

  }
}
