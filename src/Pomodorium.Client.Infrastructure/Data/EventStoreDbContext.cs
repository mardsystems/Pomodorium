using Microsoft.EntityFrameworkCore;
using System.DomainModel.EventStore;

namespace Pomodorium.Data;

public class EventStoreDbContext : DbContext
{
    public DbSet<EventRecord> EventStore { get; set; }

    public EventStoreDbContext()
    {

    }

    public EventStoreDbContext(DbContextOptions<EventStoreDbContext> options)
        : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        //optionsBuilder
        //    //.UseLazyLoadingProxies()
        //    
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EventRecord>()
            .HasKey(p => new { p.Name, p.Version });

        modelBuilder.Entity<EventRecord>()
            .Property(p => p.Date);

        modelBuilder.Entity<EventRecord>()
            .Property(p => p.Data);

        //modelBuilder.Entity<EventStoreTable>()
        //    .Property(p => p.Name)
        //    .ValueGeneratedNever();
    }
}
