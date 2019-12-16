namespace VaporStore.Data
{
    using Microsoft.EntityFrameworkCore;
    using VaporStore.Data.Models;

    public class VaporStoreDbContext : DbContext
    {
        public VaporStoreDbContext()
        {
        }

        public VaporStoreDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Card> Cards { get; set; }

        public DbSet<Purchase> Purchases { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Developer> Developers { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<GameTag> GameTags { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<User>(user =>
            {
                user
                .HasMany(u => u.Cards)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            model.Entity<Purchase>(purchase =>
            {
                purchase
                .HasOne(p => p.Card)
                .WithMany(c => c.Purchases)
                .HasForeignKey(p => p.CardId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            model.Entity<Game>(game =>
            {
                game
                .HasMany(g => g.Purchases)
                .WithOne(p => p.Game)
                .HasForeignKey(p => p.GameId)
                .OnDelete(DeleteBehavior.Restrict);

                game
                .HasOne(g => g.Developer)
                .WithMany(d => d.Games)
                .HasForeignKey(g => g.DeveloperId)
                .OnDelete(DeleteBehavior.Restrict);

                game
                .HasOne(g => g.Genre)
                .WithMany(ge => ge.Games)
                .HasForeignKey(g => g.GenreId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            model.Entity<GameTag>(gameTag =>
            {
                gameTag.HasKey(gt => new { gt.GameId, gt.TagId });

                gameTag
                .HasOne(gt => gt.Game)
                .WithMany(g => g.GameTags)
                .HasForeignKey(gt => gt.GameId)
                .OnDelete(DeleteBehavior.Restrict);

                gameTag
                .HasOne(gt => gt.Tag)
                .WithMany(t => t.GameTags)
                .HasForeignKey(gt => gt.TagId)
                .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}