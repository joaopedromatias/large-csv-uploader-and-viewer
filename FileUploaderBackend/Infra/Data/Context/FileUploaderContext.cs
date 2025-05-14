using System.Reflection;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public class FileUploaderContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Exchange> Exchanges { get; set; }
    public DbSet<Job> Jobs { get; set; }

    public FileUploaderContext(DbContextOptions<FileUploaderContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
