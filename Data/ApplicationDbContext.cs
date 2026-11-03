using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Database tables
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Theatre> Theatres { get; set; }
        public DbSet<Screen> Screens { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Show> Shows { get; set; }
        public DbSet<ShowSeat> ShowSeats { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seat table constraints
            modelBuilder.Entity<Seat>(entity =>
            {
                entity.Property(s => s.RowName)
                    .HasMaxLength(5)
                    .IsRequired();

                entity.Property(s => s.SeatType)
                    .HasMaxLength(30)
                    .IsRequired();

                entity.ToTable(table =>
                    table.HasCheckConstraint(
                        "CK_Seats_SeatNumber",
                        "[SeatNumber] > 0"));
            });

            // Show table constraints
            modelBuilder.Entity<Show>(entity =>
            {
                entity.Property(s => s.TicketPrice)
                    .HasPrecision(10, 2);

                entity.ToTable(table =>
                {
                    table.HasCheckConstraint(
                        "CK_Shows_EndTime",
                        "[EndTime] > [StartTime]");

                    table.HasCheckConstraint(
                        "CK_Shows_TicketPrice",
                        "[TicketPrice] >= 0");
                });
            });
            // ShowSeat table constraints
            modelBuilder.Entity<ShowSeat>(entity =>
            {
                entity.Property(s => s.Status)
                    .HasMaxLength(20)
                    .IsRequired();

                entity.HasOne(s => s.Show)
                    .WithMany()
                    .HasForeignKey(s => s.ShowId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(s => s.Seat)
                    .WithMany()
                    .HasForeignKey(s => s.SeatId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.ToTable(table =>
                    table.HasCheckConstraint(
                        "CK_ShowSeats_Status",
                        "[Status] IN ('Available', 'Booked')"));
            });
        }

    }
}