using AutoMapper;
using ChatApp.Core.Entities;
using ChatApp.Core.Entities.AppUserAggregate;
using ChatApp.Core.Helpers;
using ChatApp.DAL.App.Helpers;
using ChatApp.DAL.Identity;
using ChatApp.DTO;

namespace ChatApp.API.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<UserRegisterDto, ExtendedIdentityUser>();
        CreateMap<AppUserQueryParams, AppUserParameters>()
            .ForMember(
                dest => dest.NormalizedEmail,
                opt => opt.MapFrom(src => src.Email.Normalize())
            )
            .ForMember(
                dest => dest.NormalizedUserName,
                opt => opt.MapFrom(src => src.UserName.Normalize())
            )
            .ForMember(
                dest => dest.SortField,
                opt => opt.MapFrom(src => src.SortField.ToLower())
            )
            .ForMember(
                dest => dest.OrderBy,
                opt => opt.MapFrom(src => src.OrderBy == "asc"
                    ? SortDirection.Ascending
                    : SortDirection.Descending))
            .ForMember(
                dest => dest.PhoneNumber,
                opt => opt.MapFrom(src => src.Phone)
            );
        CreateMap<Avatar, AvatarDto>();
        CreateMap<AppUser, AppUserDto>();
        CreateMap<PagedList<AppUser>, PagedResponseDto<AppUserDto>>()
            .ForMember(
                dest => dest.Items,
                opt => opt.MapFrom(src => src)
            );
    }
}