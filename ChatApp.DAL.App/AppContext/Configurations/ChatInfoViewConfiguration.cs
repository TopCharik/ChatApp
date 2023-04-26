using ChatApp.Core.Entities.ChatInfoAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.DAL.App.AppContext.Configurations;

public class ChatInfoViewConfiguration : IEntityTypeConfiguration<ChatInfoView>
{
    public void Configure(EntityTypeBuilder<ChatInfoView> builder)
    {
        builder.ToView("ChatInfoView");
        builder.HasKey(x => x.ChatInfoId);
    }
}