using System.ComponentModel.DataAnnotations;

namespace TourismMVC.Models
{
    public class Destination
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        [Required]
        [StringLength(50)]
        public string? Region { get; set; }

        [DataType(DataType.Url)]
        public string? image {  get; set; }

        [StringLength(300)]
        public string? Description { get; set; }
    }
}
