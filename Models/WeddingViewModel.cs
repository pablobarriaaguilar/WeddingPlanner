namespace Weddingplaner.Models;
public class WeddingViewModel
{
    public string WedderOne { get; set; }
    public string WedderTwo { get; set; }
    public DateTime WeddingDate { get; set; }
    public int NumAsistentes { get; set; }
    public int CreatorId { get; set; }
    public int WeddingId { get; set; }

    public List<Weddingregistration> ListaAsistentes {get; set; }
}