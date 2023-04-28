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
        CreateMap<AppUser, AppUserDto>();
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
        CreateMap<Message, MessageDto>();
        CreateMap<MessageQueryParametersDto, MessageParameters>();
    }
}