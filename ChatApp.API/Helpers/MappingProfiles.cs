using AutoMapper;
using ChatApp.DAL.Identity;
using ChatApp.DTO;

namespace ChatApp.API.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<UserRegisterDto, ExtendedIdentityUser>();
    }
}