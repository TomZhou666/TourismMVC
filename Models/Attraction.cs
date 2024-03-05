using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourismMVC.Models
{
    public class Attraction
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public int DestinationId { get; set; }

        [ForeignKey("DestinationId")]
        public Destination? Destination { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        [Required]
        public string? Type { get; set; }

        [StringLength(50)]
        public string? Address { get; set; }

        [DataType(DataType.Url)]
        public string? ImageUrl { get; set; }

        [StringLength(300)]
        public string? Description { get; set; }
    }
}
