using Microsoft.EntityFrameworkCore;

namespace DocumentsOrganizer.Entities
{
    public class DocumentsOrganizerDbContext : DbContext
    {
        private string connectionString =
            "Server=DESKTOP-6607MV5\\SQLEXPRESS;Database=DocumentsOrganizerDb;Trusted_Connection=True;";
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentInformation> Informations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Document>()
                .Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(25);

            modelBuilder.Entity<DocumentInformation>()
                .Property(d => d.Description)
                .IsRequired();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
