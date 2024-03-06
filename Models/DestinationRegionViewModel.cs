using Microsoft.AspNetCore.Mvc.Rendering;

namespace TourismMVC.Models
{
    public class DestinationRegionViewModel
    {
        public List<Destination>? Destinations {  get; set; }
        public SelectList? Regions {  get; set; }
        public string? DestinationRegion {  get; set; }
        public string? SearchString { get; set; }
    }
}
