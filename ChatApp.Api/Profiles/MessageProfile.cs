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
            CreateMap<MessageDTO, MessageResponse>()
                .ReverseMap();

            CreateMap<Message, MessageDTO>()
                .ReverseMap();
        }
    }
}
