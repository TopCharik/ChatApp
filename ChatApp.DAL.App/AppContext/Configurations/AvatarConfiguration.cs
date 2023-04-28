using ChatApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.DAL.App.AppContext.Configurations;

public class AvatarConfiguration : IEntityTypeConfiguration<Avatar>
{
    public void Configure(EntityTypeBuilder<Avatar> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK_Pictures");

        builder.Property(e => e.DateSet).HasColumnType("datetime");
        builder.Property(e => e.UserId)
            .HasMaxLength(450)
            .HasColumnName("UserId");

        builder.HasOne(d => d.ChatInfo).WithMany(p => p.Avatars)
            .HasForeignKey(d => d.ChatInfoId)
            .HasConstraintName("FK_Pictures_ChatInfo");
        
        builder.HasOne(d => d.ChatInfoView).WithMany(p => p.Avatars)
            .HasForeignKey(d => d.ChatInfoId);

        builder.HasOne(d => d.User).WithMany(p => p.Avatars)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("FK_Pictures_AspNetUsers");
    }
}