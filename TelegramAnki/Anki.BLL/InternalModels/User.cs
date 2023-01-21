
using AnkiWeb;
using Anki.BLL;

namespace TelegramAnki.User
{
    public class TelegramUser
    {
        private readonly AnkiWebAPI _ankiweb;

        public long ChatID { get; set; }
        public string Cookie { get; set; } = "";
        public string CsrfToken { get; set; } = "";
        public int State { get; set; }

        public TelegramUser()
        {
            _ankiweb = new();
        }

        public async Task<(string, string)> Login(string username, string password)
        {

            if (await IsLoggedIn())
            {
                return (CsrfToken, Cookie);
            }

            return await _ankiweb.Login(
                            username: username,
                            password: password
                        );
        }

        public async Task<bool> IsLoggedIn()
        {
            return await _ankiweb.IsLoggedIn(cookie: Cookie);
        }
    }
}