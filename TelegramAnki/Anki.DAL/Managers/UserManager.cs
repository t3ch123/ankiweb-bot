using Anki.DAL.DTOs;

namespace Anki.DAL.Managers
{
    public class UserManager
    {
        public static List<UserDTO> GetUsers()
        {
            return new List<UserDTO>{
                new UserDTO { ChatID = 1, Cookie = "", State = 0 },
            };
        }

        public static UserDTO GetUser(int ChatID)
        {
            return new() { ChatID = ChatID, Cookie = "", State = 0 };
        }

        public static void UpdateUser(UserDTO user)
        {
            // todo:
        }
    }
}
