using ChatApp.Core.Entities.FooAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.DAL.AppContext.Configurations;

public class FooConfiguration : IEntityTypeConfiguration<Foo>
{
    public void Configure(EntityTypeBuilder<Foo> builder)
    {
        builder.Property(p => p.TextMessage)
            .IsRequired()
            .HasMaxLength(100);
    }
}