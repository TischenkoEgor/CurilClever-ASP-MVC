using CurilClever2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurilClever2.ViewModels
{
  public class EditUserViewModel
  {
    public User User { get; set; }
    public int? newRole { get; set; }
    
    public SelectList roles { get; set; }
    public EditUserViewModel()
    {
    }
  }
}
