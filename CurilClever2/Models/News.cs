using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CurilClever2.Models
{
  public class News
  {
    // первичный ключ в базе
    public int id { get; set; }

    // Краткое описание
    [Display(Name = "TextShort")]
    public string TextShort { get; set; }

    // Подробное описание
    [Display(Name = "TextFull")]
    public string TextFull { get; set; }

    //  текст ссылки на объект новости, если есть
    [Display(Name = "ObjectUrl")]
    public string ObjectUrl { get; set; }

    // Автор новости
    public int Userid { get; set; }
    [Display(Name = "User")]
    public User User { get; set; }

    // Время создания новости
    [Display(Name = "Created")]
    public DateTime Created { get; set; }

    // конструктор по умолчанию
    public News()
    {
      // при создании объекта записываем время создания новости из текущего времени
      Created = DateTime.Now;
    }
  }
}
