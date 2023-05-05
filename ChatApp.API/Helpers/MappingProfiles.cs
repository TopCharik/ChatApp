using AutoMapper;
using ChatApp.Core.Entities;
using ChatApp.Core.Entities.AppUserAggregate;
using ChatApp.Core.Entities.ChatInfoAggregate;
using ChatApp.Core.Entities.MessageArggregate;
using ChatApp.Core.Helpers;
using ChatApp.DAL.App.Helpers;
using ChatApp.DAL.Identity;
using ChatApp.DTO;

namespace ChatApp.API.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<RegisterAppUserDto, ExtendedIdentityUser>();
        CreateMap<AppUserQueryParams, AppUserParameters>()
            .ForMember(
                dest => dest.SortDirection,
                opt => opt.MapFrom(src => src.OrderBy == "asc"
                    ? SortDirection.Ascending
                    : SortDirection.Descending))
            .ForMember(
                dest => dest.PhoneNumber,
                opt => opt.MapFrom(src => src.Phone)
            );
        CreateMap<Avatar, AvatarDto>();
        CreateMap<AppUser, AppUserDto>()
            .ForMember(
                dest => dest.Age,
                opt => opt.MapFrom(
                    x => x.Birthday == null || x.Birthday > DateTime.Now 
                        ? null
                        : new DateTime((DateTime.Now - x.Birthday).Value.Ticks).Year.ToString() + " y.o.")
                )
            .ForMember(
                dest => dest.isOnline,
                opt => opt.MapFrom(src => src.CallHubConnectionId != null)
                );
        CreateMap<ExtendedIdentityUser, AppUserDto>();
        CreateMap<PagedList<AppUser>, PagedResponseDto<AppUserDto>>()
            .ForMember(
                dest => dest.Items,
                opt => opt.MapFrom(src => src)
            );
        CreateMap<Conversation, ConversationDto>();
        CreateMap<ChatInfo, ChatInfoDto>();
        CreateMap<PagedList<Conversation>, PagedResponseDto<ConversationDto>>()
            .ForMember(
                dest => dest.Items,
                opt => opt.MapFrom(src => src)
            );
        CreateMap<ChatInfoView, ChatInfoDto>();
        CreateMap<PagedList<ChatInfoView>, PagedResponseDto<ChatInfoDto>>()
            .ForMember(
                dest => dest.Items,
                opt => opt.MapFrom(src => src)
            );
        CreateMap<ChatInfoQueryParams, ChatInfoParameters>()
            .ForMember(
                dest => dest.SortDirection,
                opt => opt.MapFrom(src => src.OrderBy == "asc"
                    ? SortDirection.Ascending
                    : SortDirection.Descending));
        CreateMap<Participation, ParticipationDto>();
        CreateMap<Conversation, ConversationParticipationDto>()
            .ForMember(
                dest => dest.Participation,
                opt => opt.MapFrom(src => src.Participations.FirstOrDefault())
            );
        CreateMap<NewMessageDto, Message>()
            .ForMember(
                dest => dest.DateSent,
                opt => opt.MapFrom(src => DateTime.Now)
            );
        CreateMap<NewAvatarDto, Avatar>()
            .ForMember(
                dest => dest.DateSet,
                opt => opt.MapFrom(src => DateTime.Now)
            );
        CreateMap<Message, MessageDto>();
        CreateMap<MessageQueryParametersDto, MessageParameters>();
    }
}