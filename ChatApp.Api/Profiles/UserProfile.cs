using AutoMapper;
using ChatApp.Bll.DTOs;
using ChatApp.Requests;

namespace ChatApp.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDTO, UserResponse>()
                .ForMember(dst => dst.UserName, opt => opt.MapFrom(x => x.UserName));

            CreateMap<UserResponse, UserDTO>();
        }
    }
}
