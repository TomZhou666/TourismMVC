using Microsoft.AspNetCore.Mvc.Rendering;

namespace TourismMVC.Models
{
    public class AttractionTypeViewModel
    {
        public List<Attraction>? Attractions { get; set; }
        public SelectList? Types { get; set; }
        public string? AttractionType { get; set; }
        public string? SearchString { get; set; }
    }
}
