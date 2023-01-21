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

        public UserDTO GetUser(long ChatID)
        {
            using (var connection = new NpgsqlConnection(PgConnectionString))
            {
                return connection.QuerySingle<UserDTO>(
                    "SELECT * FROM \"User\" WHERE ChatId = @ChatId",
                    param: new { ChatId = ChatID }
                );
            };
        }

        public bool UserExists(long ChatID)
        {
            using (var connection = new NpgsqlConnection(PgConnectionString))
            {
                return connection.QuerySingle<bool>(
                    "SELECT COUNT(1) FROM \"User\" WHERE ChatId = @ChatId",
                    param: new { ChatId = ChatID }
                );
            }
        }

        public void UpdateUser(UserDTO user)
        {
            using (var connection = new NpgsqlConnection(PgConnectionString))
            {
                connection.Execute(
                    "UPDATE \"User\" SET Cookie = @Cookie, State = @State WHERE ChatId = @ChatId",
                    param: new { ChatId = user.ChatID, Cookie = user.Cookie, State = user.State }
                );
            }
        }

        public void CreateUser(UserDTO user)
        {
            using (var connection = new NpgsqlConnection(PgConnectionString))
            {
                connection.Execute(
                    @"
                        INSERT INTO ""User""
                            (ChatId, Cookie, State, CsrfToken)
                        VALUES
                            (@ChatID, @Cookie, @State, @CsrfToken)
                    ",
                    param: new
                    {
                        ChatID = user.ChatID,
                        Cookie = user.Cookie,
                        State = user.State,
                        CsrfToken = user.CsrfToken
                    }
                );
            }
        }
    }
}
