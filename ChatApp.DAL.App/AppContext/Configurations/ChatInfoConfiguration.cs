using ChatApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.DAL.App.AppContext.Configurations;

public class ChatInfoConfiguration : IEntityTypeConfiguration<ChatInfo>
{
    public void Configure(EntityTypeBuilder<ChatInfo> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK_ChatInfo");

        builder.Property(e => e.ChatLink).HasMaxLength(128);
        builder.Property(e => e.Title).HasMaxLength(256);
    }
}