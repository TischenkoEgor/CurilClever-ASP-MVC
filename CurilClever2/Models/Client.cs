using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CurilClever2.Models
{
  public class Client
  {
    public int id { get; set; }

    [Required(ErrorMessage = "Укажите имя")]
    [MinLength(1, ErrorMessage = "не может быть короче 1 символа")]
    [Display(Name = "Имя")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Укажите фамилию")]
    [MinLength(1, ErrorMessage = "не может быть короче 3 символа")]
    [Display(Name = "Фамилия")]
    public string SecondName { get; set; }

    [NotMapped]
    public string FIO { get { return GetFullName();} }

    [Required(ErrorMessage = "Укажите дату рождения")]
    [DataType(DataType.Date)]
    [Display(Name = "Дата рождения")]
    public DateTime DateOfBirthday { get; set; }

    [Required(ErrorMessage = "Укажите пол")]
    [Display(Name = "Пол")]
    public Gender Gender { get; set; }

    [Required(ErrorMessage = "Укажите телефон")]
    [Phone]
    [Display(Name = "Телефон")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "Укажите адрес емейл")]
    [EmailAddress]
    [Display(Name = "Электронная почта")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Укажите пасспорт")]
    [MinLength(3, ErrorMessage = "имя отеля не может быть короче 3 символов")]
    [Display(Name = "Паспорт (серия, номер кем и когда выдан)")]
    public string PassportData { get; set; }
    public string GetFullName()
    {
      return this.FirstName + " " + this.SecondName;
    }
    public int GetYearsFromBirth()
    {
      if (this.DateOfBirthday.Date >= DateTime.Now.Date)
        return 0;
      return (new DateTime(1, 1, 1) + (DateTime.Now - this.DateOfBirthday)).Year - 1;
    }
    public string GetGenderName()
    {
      if (Gender == Gender.Female)
        return "жен.";
      else
        return "муж.";
    }
  }

  public enum Gender
  {
    [Display(Name = "Муж.")]
    Male,
    [Display(Name = "Жен.")]
    Female
  }
}
