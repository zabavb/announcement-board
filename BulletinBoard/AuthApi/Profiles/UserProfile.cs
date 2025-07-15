using AuthApi.Models;
using AutoMapper;
using Library.Models.Dto;

namespace AuthApi.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
    }
}