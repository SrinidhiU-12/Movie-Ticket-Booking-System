using System.ComponentModel.DataAnnotations;

namespace MovieTicketBooking.DTOs.Seats
{
    public class BulkCreateSeatsRequest
    {
        [Range(1, int.MaxValue)]
        public int ScreenId { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one seat is required.")]
        public List<SeatRequest> Seats { get; set; } = new();
    }

    public class SeatRequest
    {
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