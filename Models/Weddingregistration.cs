using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Weddingplaner.Models;

public class Weddingregistration{
    [Key]
    public int WeddingregistrationId { get; set; }
    [ForeignKey("WeddingId")]
    public int WeddingId { get; set; }

    [ForeignKey("UsuarioId")]
    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }    
    public Wedding? Wedding { get; set; }
}