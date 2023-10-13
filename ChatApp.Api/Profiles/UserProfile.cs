using AutoMapper;
using ChatApp.Bll.DTOs;
using ChatApp.Response;

namespace ChatApp.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDTO, UserResponse>()
                .ReverseMap();

            CreateMap<UserResponse, UserDTO>();
        }
    }
}
