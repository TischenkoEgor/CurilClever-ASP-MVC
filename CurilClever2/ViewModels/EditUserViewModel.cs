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
    // ссылка на пользователя
    public User User { get; set; }
    public int? newRole { get; set; }

    //список доступных ролей для пользователя
    public SelectList roles { get; set; }

    // пустой конструктор, лень удалить, он не нужен
    public EditUserViewModel()
    {
    }
  }
}
