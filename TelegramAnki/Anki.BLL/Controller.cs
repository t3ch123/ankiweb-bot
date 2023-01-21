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

        public List<TelegramUser> GetUsers()
        {
            List<UserDTO> users = userManager.GetUsers();
            return mapper.Map<List<UserDTO>, List<TelegramUser>>(users);
        }

        public TelegramUser GetUser(long ChatID)
        {
            UserDTO user = userManager.GetUser(ChatID);
            return mapper.Map<UserDTO, TelegramUser>(user);
        }

        public bool UserExists(long ChatID)
        {
            return userManager.UserExists(ChatID);
        }

        public void UpdateUser(TelegramUser user)
        {
            UserDTO userDTO = mapper.Map<TelegramUser, UserDTO>(user);
            userManager.UpdateUser(userDTO);
        }
        public void CreateUser(TelegramUser user)
        {
            UserDTO userDTO = mapper.Map<TelegramUser, UserDTO>(user);
            userManager.CreateUser(userDTO);
        }
    }
}