
using System.ComponentModel.DataAnnotations;

namespace MovieTicketBooking.DTOs.Seats
{
    public class UpdateSeatRequest
    {
        [Range(1, int.MaxValue)]
        public int ScreenId { get; set; }

        [Required]
        [StringLength(5, MinimumLength = 1)]
        public string RowName { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int SeatNumber { get; set; }

        [Required]
        [RegularExpression("^(Regular|Premium)$",
            ErrorMessage = "SeatType must be Regular or Premium.")]
        public string SeatType { get; set; } = string.Empty;
    }
}


