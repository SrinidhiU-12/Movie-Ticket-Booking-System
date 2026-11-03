
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
        

        // Seat table constraints
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<Seat>(entity =>
        //    {
        //        entity.Property(s => s.RowName)
        //            .HasMaxLength(5)
        //            .IsRequired();

        //        entity.Property(s => s.SeatType)
        //            .HasMaxLength(30)
        //            .IsRequired();

        //        entity.ToTable(table =>
        //            table.HasCheckConstraint(
        //                "CK_Seats_SeatNumber",
        //                "[SeatNumber] > 0"));
        //    });
        //}
    }
}
