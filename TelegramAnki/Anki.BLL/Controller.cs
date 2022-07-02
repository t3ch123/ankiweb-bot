using Anki.DAL.DTOs;
using Anki.DAL.Managers;
using AutoMapper;
using TelegramAnki.Settings;
using TelegramAnki.User;

namespace Anki.BLL
{

    public class Controller
    {
        private readonly MapperConfiguration config;
        private readonly IMapper mapper;
        private readonly UserManager userManager;

        public Controller(Settings settings)
        {
            config = new(cfg => { cfg.AddProfile<MapperProfile>(); });
            mapper = (Mapper)config.CreateMapper();
            userManager = new UserManager(settings: settings);
        }

        public List<User> GetUsers()
        {
            List<UserDTO> users = userManager.GetUsers();
            return mapper.Map<List<UserDTO>, List<User>>(users);
        }

        public User GetUser(int ChatID)
        {
            UserDTO user = userManager.GetUser(ChatID);
            return mapper.Map<UserDTO, User>(user);
        }

        public void UpdateUser(User user)
        {
            UserDTO userDTO = mapper.Map<User, UserDTO>(user);
            userManager.UpdateUser(userDTO);
        }
    }
}