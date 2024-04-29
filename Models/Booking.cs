using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourismMVC.Models
{
    public class Booking
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public IdentityUser? User { get; set; }
        [Required]
        public int AttractionId { get; set; }
        [ForeignKey("AttractionId")]
        public Attraction? Attraction { get; set; }
        
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }
        [DataType(DataType.Date)]
        public DateTime? CreatedDate { get; set; }
        public string? Status { get; set; }
    }
}
