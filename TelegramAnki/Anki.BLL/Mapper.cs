using Anki.DAL.DTOs;
using AutoMapper;
using TelegramAnki.User;

namespace Anki.BLL
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserDTO, TelegramUser>();
            CreateMap<TelegramUser, UserDTO>();
        }
    }
}
