using StateMachine;
using TelegramAnki.User;
using AnkiWeb;

namespace Anki.BLL
{
    public interface ICommandsRepository
    {
        public Task ExecuteCommand(Command cmd, TelegramUser user);
    }

    public class CommandsRepository : ICommandsRepository
    {
        Controller _controller;
        public CommandsRepository(Controller controller)
        {
            _controller = controller;
        }

        private async Task Login(TelegramUser user)
        {
            string username, password;
            string csrfToken, sessionToken;

            if (await user.IsLoggedIn())
            {
                /* If user is already logged in, we don't need to ask for credentials */
                Console.WriteLine("Login: User {0} is already logged in", user.ChatID);
                return;
            }

            (username, password) = await AskForCredentials(user);
            (csrfToken, sessionToken) = await user.Login(username, password);

            Console.WriteLine("csrfToken   : {0}", csrfToken);
            Console.WriteLine("sessionToken: {0}", sessionToken);

            user.CsrfToken = csrfToken;
            user.Cookie = sessionToken;

            _controller.UpdateUser(user);
        }

        private async Task ViewDecks(TelegramUser user)
        {
            if (!await user.IsLoggedIn())
            {
                System.Console.WriteLine("ViewDecks: User {0} is not logged in", user.ChatID);
                return;
            }

            System.Console.WriteLine("Handler: executing command ViewDecks");
            await user.GetDecks();
        }

        private Task<(string, string)> AskForCredentials(TelegramUser user)
        {
            return Task.FromResult(
                ("lyamets.misha@gmail.com", "VR48hwmepuf6XE")
            );
        }

        public Task ExecuteCommand(Command cmd, TelegramUser user)
        {
            switch (cmd)
            {
                case Command.Login:
                    return Login(user);

                case Command.ViewDecks:
                    return ViewDecks(user);

                default:
                    throw new NotImplementedException();
            }

        }
    }
}