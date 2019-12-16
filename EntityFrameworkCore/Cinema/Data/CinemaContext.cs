namespace Cinema.Data
{
    using Cinema.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class CinemaContext : DbContext
    {
        public CinemaContext()  { }

        public CinemaContext(DbContextOptions options)
            : base(options)   { }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<Seat> Seats { get; set; }

        public DbSet<Hall> Halls { get; set; }

        public DbSet<Projection>  Projections{ get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>(ticket =>
            {
                ticket
                .HasOne(t => t.Customer)
                .WithMany(c => c.Tickets)
                .HasForeignKey(t => t.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Hall>(hall =>
            {
                hall.
                    HasMany(h => h.Seats)
                    .WithOne(s => s.Hall)
                    .HasForeignKey(s => s.HallId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Projection>(projection =>
            {
                projection
                .HasOne(p => p.Movie)
                .WithMany(m => m.Projections)
                .HasForeignKey(p => p.MovieId)
                .OnDelete(DeleteBehavior.Restrict);

                projection
                .HasOne(p => p.Hall)
                .WithMany(h => h.Projections)
                .HasForeignKey(p => p.HallId)
                .OnDelete(DeleteBehavior.Restrict);

                projection
                .HasMany(p => p.Tickets)
                .WithOne(t => t.Projection)
                .HasForeignKey(t => t.ProjectionId)
                .OnDelete(DeleteBehavior.Restrict);

            });
        }
    }
}