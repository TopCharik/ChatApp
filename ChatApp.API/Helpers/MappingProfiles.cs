using AutoMapper;
using ChatApp.API.DTOs;
using ChatApp.DAL.Identity;

namespace ChatApp.API.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<UserRegisterDto, ExtendedIdentityUser>();
    }
}