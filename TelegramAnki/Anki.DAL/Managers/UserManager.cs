using Anki.DAL.DTOs;
using Dapper;
using Npgsql;
using TelegramAnki.Settings;

namespace Anki.DAL.Managers
{
    public class UserManager
    {
        readonly string PgConnectionString;

        public UserManager(Settings settings)
        {
            PgConnectionString = settings.PgConnectionString;
        }

        public List<UserDTO> GetUsers()
        {
            using (var connection = new NpgsqlConnection(PgConnectionString))
            {
                return connection.Query<UserDTO>(
                    "SELECT * FROM \"User\"",
                    commandType: System.Data.CommandType.Text
                ).ToList();
            };
        }

        public UserDTO GetUser(int ChatID)
        {
            return new() { ChatID = ChatID, Cookie = "", State = 0 };
        }

        public void UpdateUser(UserDTO user)
        {
            // todo:
        }
    }
}
