using ChatApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.DAL.App.AppContext.Configurations;

public class ParticipationConfiguration : IEntityTypeConfiguration<Participation>
{
    public void Configure(EntityTypeBuilder<Participation> builder)
    {
        builder.HasIndex(e => new {e.AspNetUserId, e.ConversationId}, "QK_Participations_AspNetUserId_ConversationId")
            .IsUnique();

        builder.HasOne(d => d.AppUser).WithMany(p => p.Participations)
            .HasForeignKey(d => d.AspNetUserId)
            .HasConstraintName("FK_Participations_AspNetUsers");

        builder.HasOne(d => d.Conversation).WithMany(p => p.Participations)
            .HasForeignKey(d => d.ConversationId)
            .HasConstraintName("FK_Participations_Conversations");
    }
}