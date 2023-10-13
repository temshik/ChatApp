using AutoMapper;
using ChatApp.Bll.DTOs;
using ChatApp.Response;

namespace ChatApp.Profiles
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<RoomDTO, RoomResponse>()
                .ReverseMap();

            CreateMap<RoomResponse, RoomDTO>();
        }
    }
}
