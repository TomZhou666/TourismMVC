using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourismMVC.Models
{
    public class Booking
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [ForeignKey("UserId")]
        public IdentityUser? User { get; set; }
        [Required]
        public int AttractionId { get; set; }
        [ForeignKey("AttractionId")]
        public Attraction? Attraction { get; set; }
        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }
        [DataType(DataType.Date)]
        public DateTime? CreatedDate { get; set; }
        public enum BookingStatus { Pending, Confirmed, Cancelled, Completed }
        public BookingStatus Status { get; set; }

    }
}
