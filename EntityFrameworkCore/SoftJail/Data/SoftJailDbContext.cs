namespace SoftJail.Data
{
	using Microsoft.EntityFrameworkCore;
    using SoftJail.Data.Models;

    public class SoftJailDbContext : DbContext
	{
		public SoftJailDbContext()
		{
		}

		public SoftJailDbContext(DbContextOptions options)
			: base(options)
		{
		}

        public DbSet<Department> Departments { get; set; }

        public DbSet<Cell> Cells { get; set; }

        public DbSet<Mail> Mails { get; set; }

        public DbSet<Prisoner> Prisoners { get; set; }

        public DbSet<Officer> Officers { get; set; }

        public DbSet<OfficerPrisoner> OfficersPrisoners { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder
					.UseSqlServer(Configuration.ConnectionString);
			}
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
            builder.Entity<Cell>(cell =>
            {
                cell
                    .HasOne(c => c.Department)
                    .WithMany(d => d.Cells)
                    .HasForeignKey(c => c.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Prisoner>(prisoner =>
            {
                prisoner
                    .HasOne(p => p.Cell)
                    .WithMany(c => c.Prisoners)
                    .HasForeignKey(p => p.CellId)
                    .OnDelete(DeleteBehavior.Restrict);

                prisoner
                    .HasMany(p => p.Mails)
                    .WithOne(m => m.Prisoner)
                    .HasForeignKey(m => m.PrisonerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            
            builder.Entity<OfficerPrisoner>(officerPrisoner =>
            {
                officerPrisoner.HasKey(op => new { op.OfficerId, op.PrisonerId });

                officerPrisoner
                    .HasOne(op => op.Officer)
                    .WithMany(o => o.OfficerPrisoners)
                    .HasForeignKey(op => op.OfficerId)
                    .OnDelete(DeleteBehavior.Restrict);

                officerPrisoner
                    .HasOne(op => op.Prisoner)
                    .WithMany(p => p.PrisonerOfficers)
                    .HasForeignKey(op => op.PrisonerId)
                    .OnDelete(DeleteBehavior.Restrict);
                
            });
		}
	}
}