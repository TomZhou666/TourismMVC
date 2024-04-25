using System.ComponentModel.DataAnnotations;

namespace TourismMVC.Models
{
    public class TourPlan
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public String? Title { get; set; }
        [Required]
        public int DestinationId { get; set; }
        public Destination? Destination { get; set; }
        [Required]
        public List<int>? AttractionIds { get; set; }
        public List<Attraction>? Attractions { get; set; }
        [Required]
        public int Duration { get; set; }
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public String? Description { get; set; }
    }
}
