#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace Weddingplaner.Models;
// using statements and namespace go here
public class LoginUser
{
    // No other fields!
    [Required]    
    [EmailAddress]
    public string Email { get; set; }    
    [Required]    
    public string Password { get; set; } 
}
