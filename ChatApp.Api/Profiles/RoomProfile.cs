using AutoMapper;
using ChatApp.Bll.DTOs;
using ChatApp.DAL.Entities;
using ChatApp.Requests;

namespace ChatApp.Profiles
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<RoomDTO, RoomResponse>()
                .ForMember(dst => dst.Admin, opt => opt.MapFrom(x => x.Admin.UserName));

            CreateMap<RoomResponse, RoomDTO>();
        }
    }
}
