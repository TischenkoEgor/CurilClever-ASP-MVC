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

    [Display(Name = "CreationDate")]
    public DateTime CreationDate { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "BeginTravelDate")]
    public DateTime BeginTravelDate { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "EndTravelDate")]
    public DateTime EndTravelDate { get; set; }

    [Required]
    [Display(Name = "Hotel")]
    public int Hotelid { get; set; }
    [Display(Name = "Hotel")]
    public Hotel Hotel { get; set; }

    [Required]
    [Display(Name = "Client")]
    public int Clientid { get; set; }
    [Display(Name = "Client")]
    public Client Client { get; set; }

    [Required]
    [Display(Name = "Price")]
    [Range(0, int.MaxValue)]
    public int Price { get; set; }

    [Display(Name = "TotalPaid")]
    [Range(0, int.MaxValue)]
    public int TotalPaid { get; set; }

    public IEnumerable<OrderComment> Comments { get; set; }

    [Required]
    [Display(Name = "PayStatus")]
    public PayStatus PayStatus { get; set; }
  }
  public enum PayStatus
  {
    [Display(Name = "Unpaid")]
    Unpaid,
    [Display(Name = "PrePayed")]
    PrePayed,
    [Display(Name = "Paid")]
    Paid,
    [Display(Name = "Custom")]
    Custom
  }
}
