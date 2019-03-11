using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurilClever2.Models
{
  public class SampleData
  {
    public static void Initialize(CleverDBContext context)
    {
      if (!context.Users.Any())
      {
        context.Users.AddRange(
            new User
            {
              name = "Admin",
              Login = "Admin",
              PasswordHash = CryptoHelper.GetMD5("123"),
              AccessLevel = 0
            },
            new User
            {
              name = "Loh",
              Login = "Loh",
              PasswordHash = CryptoHelper.GetMD5("123"),
              AccessLevel = 9000
            }
        );
        context.SaveChanges();
      }
      if(!context.Hotels.Any())
      {
        context.Hotels.AddRange(
            new Hotel
            {
              Name = "Default",
              Price = 10
            }
          );
      }
    }
  }
}
