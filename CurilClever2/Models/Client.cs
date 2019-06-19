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

    [Required(ErrorMessage = "FirstNameRequired")]
    [MinLength(1, ErrorMessage = "FirstNameMinLength")]
    [Display(Name = "FirstName")]
    [RegularExpression(@"^[a-zA-Zа-яА-Я]{1,}$", ErrorMessage = "FirstNameWrongName")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "SecondNameRequred")]
    [MinLength(2, ErrorMessage = "SecondNameMinLength")]
    [Display(Name = "SecondName")]
    [RegularExpression(@"^[a-zA-Zа-яА-Я]{1,}-?[a-zA-Zа-яА-Я]{1,}$", ErrorMessage = "SecondNameWrongName")]
    public string SecondName { get; set; }

    [NotMapped]
    public string FIO { get { return GetFullName();} }

    [Required(ErrorMessage = "DateOfBirthdayReqired")]
    [DataType(DataType.Date)]
    [Display(Name = "DateOfBirthday")]
    public DateTime DateOfBirthday { get; set; }

    [Required(ErrorMessage = "GenderRequired")]
    [Display(Name = "Gender")]
    public Gender Gender { get; set; }

    [Required(ErrorMessage = "PhoneRequired")]
    [DataType(DataType.PhoneNumber, ErrorMessage = "PhoneWrong")]
    [RegularExpression(@"^((8|\+7)[\- ]?)?(\(?\d{3,4}\)?[\- ]?)?[\d\- ]{5,10}$", ErrorMessage = "PhoneInvalid")]
    [Display(Name = "Phone")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "EmailRequired")]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "PassportDataRequired")]
    [MinLength(8, ErrorMessage = "PassportDataMinLength")]
    [Display(Name = "PassportData")]
    public string PassportData { get; set; }
    public string GetFullName()
    {
      return this.FirstName + " " + this.SecondName;
    }

    public IEnumerable<ClientComment> Comments {get; set;}
    public Client()
    {
      Comments = new List<ClientComment>();
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
    [Display(Name = "Male")]
    Male,
    [Display(Name = "Female")]
    Female
  }
}
