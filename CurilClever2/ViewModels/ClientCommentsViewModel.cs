using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurilClever2.Models;
namespace CurilClever2.ViewModels
{
  public class ClientCommentsViewModel
  {
    public int clientid { get; set;}
    public IEnumerable<ClientComment> Comments { get; set; }
    public PageViewModel PageViewModel { get; set; }

  }
}
