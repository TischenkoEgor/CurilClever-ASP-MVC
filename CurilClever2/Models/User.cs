using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CurilClever2.Models
{
  public class User
  {
    public int id { get; set; }
    [MinLength(1, ErrorMessage = "NameMinLength")]
    [Display(Name = "Name")]
    [RegularExpression(@"^[a-zA-Zа-яА-Я ]{1,}$", ErrorMessage = "NameWrongName")]
    public string name { get; set; }
    public string Login { get; set; }
    public string PasswordHash { get; set; }

    public int? RoleId { get; set; }
    public Role Role { get; set; }

    public bool checkPassword(string pass)
    {
      if (CryptoHelper.GetMD5(pass) == PasswordHash)
        return true;
      else
        return false;
    }

    public int AccessLevel { get; set; }
  }
  public class Role
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public List<User> Users { get; set; }
    public Role()
    {
      Users = new List<User>();
    }
  }
}
