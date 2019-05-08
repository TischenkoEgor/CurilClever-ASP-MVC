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

      if (!context.Roles.Any())
      {
        context.Roles.AddRange(
          new Role { Name = "Admin" },
          new Role { Name = "Moderator" },
          new Role { Name = "Manager" },
          new Role { Name = "DefaultUser"});
        context.SaveChanges();
      }
      if (!context.Users.Any())
      {
        Role adminRole = context.Roles.Where(x => x.Name == "Admin").FirstOrDefault();
        context.Users.AddRange(
            new User
            {
              name = "Admin",
              Login = "Admin",

              PasswordHash = CryptoHelper.GetMD5("123"),
              AccessLevel = 0,
              Role = adminRole
            }
        );
        context.SaveChanges();
      }
      if (!context.Hotels.Any())
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
