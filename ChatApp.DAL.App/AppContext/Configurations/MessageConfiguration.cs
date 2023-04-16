using ChatApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.DAL.App.AppContext.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.Property(e => e.DateSent).HasColumnType("datetime");
        builder.Property(e => e.MessageText).HasMaxLength(4000);
        
        builder.HasOne(d => d.Participation).WithMany(p => p.Messages)
            .HasForeignKey(d => d.ParticipationId)
            .HasConstraintName("FK_Messages_Participations");

        builder.HasOne(d => d.ReplyToNavigation).WithMany(p => p.InverseReplyToNavigation)
            .HasForeignKey(d => d.ReplyTo);
    }
}