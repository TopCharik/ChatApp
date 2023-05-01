using AutoMapper;
using ChatApp.DTO;

namespace ChatApp.BlazorServer.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<AppUserDto, EditUserDto>();
    }
}