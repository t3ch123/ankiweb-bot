

using Anki.DAL.DTOs;
using Anki.DAL.Managers;
using AutoMapper;
using TelegramAnki.User;

namespace Anki.BLL
{

    public class Controller
    {
        private readonly MapperConfiguration config;
        private readonly Mapper mapper;

        public Controller()
        {
            config = new(cfg => { cfg.AddProfile<MapperProfile>(); });
            mapper = (Mapper)config.CreateMapper();
        }

        public List<User> GetUsers()
        {
            List<UserDTO> users = UserManager.GetUsers();
            return mapper.Map<List<UserDTO>, List<User>>(users);
        }

        public User GetUser(int ChatID)
        {
            UserDTO user = UserManager.GetUser(ChatID);
            return mapper.Map<UserDTO, User>(user);
        }

        public void UpdateUser(User user)
        {
            UserDTO userDTO = mapper.Map<User, UserDTO>(user);
            UserManager.UpdateUser(userDTO);
        }
    }
}