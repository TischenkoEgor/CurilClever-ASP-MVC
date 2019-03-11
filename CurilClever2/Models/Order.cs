using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CurilClever2.Models
{
  public class Order
  {
    public int id { get; set; }
    public DateTime CreationDate { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Дата начала путешествия")]
    public DateTime BeginTravelDate { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Дата конца путешествия")]
    public DateTime EndTravelDate { get; set; }

    [Required]
    [Display(Name = "Отель")]
    public int Hotelid { get; set; }
    public Hotel Hotel { get; set; }

    [Required]
    [Display(Name = "Клиент")]
    public int Clientid { get; set; }
    public Client Client { get; set; }

    [Required]
    [Display(Name = "Стоимость тура")]
    [Range(0, int.MaxValue)]
    public int Price { get; set; }

    [Display(Name = "Оплачено")]
    [Range(0, int.MaxValue)]
    public int TotalPaid { get; set; }

    [Required]
    [Display(Name = "Статус оплаты")]
    public PayStatus PayStatus { get; set; }
  }
  public enum PayStatus
  {
    [Display(Name = "не оплачено")]
    Unpaid,
    [Display(Name = "внесена предоплата")]
    PrePayed,
    [Display(Name = "оплачено полностью")]
    Paid,
    [Display(Name = "иное")]
    Custom
  }
}
