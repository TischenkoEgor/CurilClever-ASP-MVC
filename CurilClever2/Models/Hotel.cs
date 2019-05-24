using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CurilClever2.Models
{
  public class Hotel
  {

    public int id { get; set; }
    [Required(ErrorMessage = "Не указано название отеля")]
    [MinLength(3, ErrorMessage = "имя отеля не может быть короче 3 символов")]
    [Display(Name = "Название отеля")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Укажите адрес отеля")]
    [MinLength(9, ErrorMessage = "Адрес отеля не может быть короче 9 символов")]
    [Display(Name = "Адрес отеля")]
    public string Addres { get; set; }
    
    [Required(ErrorMessage = "Укажите количество звезд отеля")]
    [Range(1, 5, ErrorMessage = "Количество звезд строго от 1 до 5")]
    [Display(Name = "Количество звезд отеля")]
    public int? StarsRate { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Цена не может быть меньше нуля")]
    [Required(ErrorMessage = "Укажите цену в отеле")]
    [Display(Name = "Стоимость ночи")]
    public int? Price { get; set; }

    public double X { get; set; }
    public double Y { get; set; }
    public int Zoom { get; set; }
  }
}
