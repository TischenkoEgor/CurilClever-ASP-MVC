using CurilClever2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurilClever2.ViewModels
{
  public class ClientPageViewModel
  {
    public IEnumerable<Client> Clients { get; set; }
    public PageViewModel PageViewModel { get; set; }
  }
}
