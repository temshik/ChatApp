using AutoMapper;
using ChatApp.Bll.DTOs;
using ChatApp.DAL.Entities;
using ChatApp.Requests;
using ChatApp.Response;

namespace ChatApp.Profiles
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<MessageDTO, MessageResponse>();
            CreateMap<Message, MessageDTO>()
                .ForMember(dst => dst.Room, opt => opt.MapFrom(x => x.ToRoom.Name));
            CreateMap<MessageRequest, MessageDTO>();
        }
    }
}
