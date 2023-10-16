using AutoMapper;
using ChatApp.Bll.DTOs;
using ChatApp.DAL.Entities;
using ChatApp.Requests;
using ChatApp.Response;

namespace ChatApp.Profiles
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<RoomDTO, RoomResponse>();
            CreateMap<Room, RoomDTO>()
                .ForMember(dst => dst.Admin, opt => opt.MapFrom(x => x.Admin.Name));
            CreateMap<RoomRequest, RoomDTO>();
        }
    }
}
