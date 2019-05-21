using CurilClever2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CurilClever2.ViewModels
{
  public class ManageAccountViewModel
  {
    [Display(Name = "Имя")]
    [Required(ErrorMessage = "Не указан Имя")]
    public string Name { get; set; }

    [Display(Name = "EMail")]
    [Required(ErrorMessage = "Не указан Email")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Введите корректный email")]
    public string Login { get; set; }

    [Display(Name = "новый пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "повторите пароль")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string ConfirmPassword { get; set; }

    [Display(Name = "Подписка на новости и обновления")]
    public bool EMailNewsSubscribe { get; set; }

    public ManageAccountViewModel()
    {

    }
    public ManageAccountViewModel(User user, Subscribe subscribe )
    {
      Name = user.name;
      Login = user.Login;
      EMailNewsSubscribe = subscribe.SendNews;
    }
  }
}
