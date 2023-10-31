using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Weddingplaner.Models;

public class Wedding{
    [Key]
public int WeddingId { get; set;}
[Required]
public string WedderOne { get; set; }
[Required]
public string WedderTwo { get; set; }
[BeforeDate]
[Required]
public DateTime WeddingDate { get; set;}
[Required]
public string Address { get; set; }

public DateTime CreatedAt { get; set; } = DateTime.Now;
public DateTime UpdatedAt { get; set; } = DateTime.Now;

public int UsuarioId { get; set; }
public Usuario? Creator { get; set; }

public List<Weddingregistration> Asistents { get; set; } = new List<Weddingregistration>();



public class BeforeDateAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            // Si el valor es nulo, se considera como requerido y se lanza un error
            return new ValidationResult("Date is required!");
        }

        DateTime inputValue = DateTime.Parse(value.ToString());
        DateTime currentDate = DateTime.Now;

        if (inputValue <= currentDate)
        {
            // La fecha debe ser posterior a la fecha actual, de lo contrario, lanza un error
            return new ValidationResult("The date must be after the current date!");
        }
        else
        {
            // Si la fecha es válida, devuelve ValidationResult.Success
            return ValidationResult.Success;
        }
    }
}


}