using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurilClever2.Models
{
  public class CaptureModel
  {
    [Key]
    public string hashstring { get; set; }
    public string code { get; set; }
    
    public void RegenerateCode()
    {
      this.code = new Random(DateTime.Now.Millisecond).Next(1111, 9999).ToString();
    }
    //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    //{
    //  List<ValidationResult> errors = new List<ValidationResult>();
    //  if(!this.userInput.Equals(this.code))
    //  {
    //    errors.Add(new ValidationResult("неправильно введена капча курильщика"));
    //  }

    //  return errors;
    //}
  }
}
