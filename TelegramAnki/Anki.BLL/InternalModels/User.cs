
using AnkiWeb;

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

        public async Task<bool> Login()
        {
            string csrfToken, sessionToken;

            (csrfToken, sessionToken) = await _ankiweb.Login(
                username: "",
                password: ""
            );

            Console.WriteLine("csrfToken   : {0}", csrfToken);
            Console.WriteLine("sessionToken: {0}", sessionToken);

            return csrfToken != "" && sessionToken != "";
        }
    }
}