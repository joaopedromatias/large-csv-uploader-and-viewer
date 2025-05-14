using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configuration;

public class ExchangeConfiguration : IEntityTypeConfiguration<Exchange>
{
    public void Configure(EntityTypeBuilder<Exchange> builder)
    {
        builder.ToTable("EXCHANGE");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("ID");

        builder.Property(x => x.CurrencyCode)
            .IsRequired()
            .HasColumnName("CURRENCY_CODE")
            .HasColumnType("VARCHAR(3)");

        builder.Property(x => x.JobId)
            .IsRequired()
            .HasColumnName("JOB_ID");

        builder.Property(x => x.RateToUsd)
            .IsRequired()
            .HasColumnName("RATE_TO_USD");         
    }
}
