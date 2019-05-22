using CurilClever2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurilClever2.ViewModels
{
  public class UserPageViewModel
  {
    // список пользователей для отображения
    public IEnumerable<User> Users { get; set; }
    // данные посртранички
    public PageViewModel PageViewModel { get; set; }
  }
}
