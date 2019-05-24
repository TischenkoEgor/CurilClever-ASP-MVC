using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurilClever2.Models
{
  public class Visit
  {
    public int id { get; set; }

    // ссылка куда был заход
    public string path { get; set; }

    // время посещения
    public DateTime dateTime { get; set; }

    //имя пользователя (если есть)
    public string userName { get; set; }

    public Visit()
    {
      dateTime = DateTime.Now;
    }

    // конструктор что бы не писать заполнение
    public Visit(string path, DateTime Date, string UserName="anonim")
    {
      this.path = path;
      dateTime = Date;
      userName = UserName; 
    }
  }
}
