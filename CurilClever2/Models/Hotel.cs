using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Localization;

namespace CurilClever2.Models
{
  public class Hotel
  {

    public int id { get; set; }
    [Required(ErrorMessage = "HotelNameRequired")]
    [MinLength(3, ErrorMessage = "MinLengthError")]
    [Display(Name = "HotelName")]
    public string Name { get; set; }

    [Required(ErrorMessage = "AdrRequired")]
    [MinLength(9, ErrorMessage = "AdrMinLength")]
    [Display(Name = "Adr")]
    public string Addres { get; set; }
    
    [Required(ErrorMessage = "StarsRateRequired")]
    [Range(1, 5, ErrorMessage = "StarsRateRange")]
    [Display(Name = "StartsRate")]
    public int? StarsRate { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "PriceRange")]
    [Required(ErrorMessage = "PriceRequired")]
    [Display(Name = "Price")]
    public int? Price { get; set; }

    public double X { get; set; }
    public double Y { get; set; }
    public int Zoom { get; set; }
  }
}
