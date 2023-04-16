using ChatApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.DAL.App.AppContext.Configurations;

public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        builder.HasKey(e => e.Id).HasName("PK_Conversation");
        
        builder.HasIndex(e => e.ChatInfoId, "QK_Conversations_ChatInfoId").IsUnique();

        builder.HasOne(d => d.ChatInfo).WithOne(p => p.Conversation)
            .HasForeignKey<Conversation>(d => d.ChatInfoId)
            .HasConstraintName("FK_Conversation_ChatInfo");
    }
}