using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CurilClever2.Models;
namespace CurilClever2.ViewModels
{
  public class RegisterModel
  {
    [Display(Name = "Имя")]
    [Required(ErrorMessage = "Не указан Имя")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Не указан Email")]
    public string Login { get; set; }

    [Required(ErrorMessage = "Не указан пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Пароль введен неверно")]
    public string ConfirmPassword { get; set; }

    [DataType(DataType.Text)]
    [Required(ErrorMessage = "не введен код капчи курильщика")]
    public string CaptureUserInput { get; set;}
    
    public string CaptureHash { get; set;}

    public string code { get; set; }

    public RegisterModel()
    {
      this.code = new Random(DateTime.Now.Millisecond).Next(1111, 9999).ToString();
      CaptureHash = CryptoHelper.GetMD5(code);
    }
    public bool CheckUserCaptureinput()
    {
      if (CaptureUserInput != null && CaptureHash.Equals(CryptoHelper.GetMD5(CaptureUserInput)))
        return true;
      else
        return false;
    }
  }
}
