using Microsoft.EntityFrameworkCore;
using TreeApp.Domain.Entities;

namespace TreeApp.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Node> Nodes => Set<Node>();
    public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<Node>(entity =>
        {
            entity.HasKey(n => n.Id);
            entity.Property(n => n.Name).IsRequired().HasMaxLength(256);
            entity.Property(n => n.TreeName).IsRequired().HasMaxLength(256);

            entity.HasMany(n => n.Children)
                .WithOne(n => n.Parent)
                .HasForeignKey(n => n.ParentId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<JournalEntry>(entity =>
        {
            entity.HasKey(j => j.Id);
            entity.Property(j => j.EventId).IsRequired();
            entity.Property(j => j.CreatedAt)
                .HasColumnType("timestamp with time zone");
            entity.Property(j => j.QueryString).HasMaxLength(2048);
            entity.Property(j => j.RequestBody);
            entity.Property(j => j.StackTrace);
            entity.Property(j => j.ExceptionType).HasMaxLength(256);
            entity.Property(j => j.ExceptionMessage);
        });
    }
}