using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CurilClever2.Models
{
  public class CryptoHelper
  {
    public static string GetMD5(string text, MD5 md5 = null)
    {
      if (md5 == null)
        md5 = MD5.Create();
      byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
      StringBuilder sb = new StringBuilder();
      for (int i = 0; i < data.Length; i++)
      {
        sb.Append(data[i].ToString("x2"));
      }
      return sb.ToString();
    }
  }
}
