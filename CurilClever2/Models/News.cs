using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurilClever2.Models
{
  public class News
  {
    // первичный ключ в базе
    public int id { get; set; }
   
    // Краткое описание
    public string TextShort { get; set; }
   
    // Подробное описание
    public string TextFull { get; set; }
  
    //  текст ссылки на объект новости, если есть
    public string ObjectUrl { get; set; }

    // Автор новости
    public int Userid { get; set; }
    public User User { get; set; }

    // Время создания новости
    public DateTime Created { get; set; }

    // конструктор по умолчанию
    public News()
    {
      // при создании объекта записываем время создания новости из текущего времени
      Created = DateTime.Now;
    }
  }
}
