using AutoMapper;
using ChatApp.Bll.DTOs;
using ChatApp.Requests;

namespace ChatApp.Profiles
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<MessageDTO, MessageResponse>()
                .ForMember(dst => dst.FromUserName, opt => opt.MapFrom(x => x.FromUser.UserName))
                .ForMember(dst => dst.FromFullName, opt => opt.MapFrom(x => x.FromUser.FullName))
                .ForMember(dst => dst.Room, opt => opt.MapFrom(x => x.ToRoom.Name))
                .ForMember(dst => dst.Avatar, opt => opt.MapFrom(x => x.FromUser.Avatar))
                .ForMember(dst => dst.Content, opt => opt.MapFrom(x => x.Content));

            CreateMap<MessageResponse, MessageDTO>();
        }
    }
}
