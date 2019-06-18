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
    [Display(Name = "Name")]
    [Required(ErrorMessage = "NameRequired")]
    [MinLength(2, ErrorMessage = "NameMinLength")]
    [RegularExpression(@"^[a-zA-Zа-яА-Я ]{1,}$", ErrorMessage = "NameWrongName")]
    public string Name { get; set; }

    [Display(Name = "Login")]
    [Required(ErrorMessage = "LoginRequired")]
    [DataType(DataType.EmailAddress, ErrorMessage = "LoginIncorrect")]
    public string Login { get; set; }

    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "ConfirmPassword")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "ConfirmPasswordCompare")]
    public string ConfirmPassword { get; set; }

    [Display(Name = "EMailNewsSubscribe")]
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
