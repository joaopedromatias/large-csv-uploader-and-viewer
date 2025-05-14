using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        
        builder.ToTable("PRODUCT");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("ID");

        builder.Property(x => x.Name)
            .IsRequired()
            .HasColumnName("NAME")
            .HasColumnType("NVARCHAR(350)")
            .IsUnicode();

        builder.Property(x => x.Price)
            .IsRequired()
            .HasColumnName("PRICE");    

        builder.Property(x => x.Expiration)
            .IsRequired()
            .HasColumnName("EXPIRATION");

        builder.Property(x => x.JobId)
            .IsRequired()
            .HasColumnName("JOB_ID");
    }
}
