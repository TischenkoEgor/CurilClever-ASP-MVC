using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurilClever2.Models
{
  public class User
  {
    public int id { get; set; }
    public string name { get; set; }
    public string Login { get; set; }
    public string PasswordHash { get; set; }

    public bool checkPassword(string pass)
    {
      if (CryptoHelper.GetMD5(pass) == PasswordHash)
        return true;
      else
        return false;
    }

    public int AccessLevel { get; set; }
  }
}
