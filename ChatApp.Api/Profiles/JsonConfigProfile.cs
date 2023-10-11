using AutoMapper;
using ChatApp.Bll.DTOs;

namespace ChatApp.Api.Profiles
{
    public class JsonConfigProfile : Profile
    {
        public JsonConfigProfile() => CreateMap<JsonConfig, JsonConfigDTO>();
    }
}
