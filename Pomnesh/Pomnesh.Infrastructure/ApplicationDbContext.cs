using Microsoft.EntityFrameworkCore;
using Pomnesh.Domain.Entity;

namespace Pomnesh.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Context> Contexts { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<Result> Results { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attachment>()
            .HasOne(a => a.Context)
            .WithMany(c => c.Attachments)
            .HasForeignKey(a => a.ContextId)
            .OnDelete(DeleteBehavior.Cascade);
        
    }
}