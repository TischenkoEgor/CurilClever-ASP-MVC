﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurilClever2.Models
{
  public class VkAccessTokenData
  {
    public string access_token { get; set; }
    public int expires_in { get; set; }
    public int user_id { get; set; }
    public string email { get; set; }
  }

  public class VkusersData
  {
    public VkontakteUser[] response { get; set; }
  }

  public class VkontakteUser
  {
    public int id { get; set; }
    public string first_name { get; set; }
    public string last_name { get; set; }
  }
}
