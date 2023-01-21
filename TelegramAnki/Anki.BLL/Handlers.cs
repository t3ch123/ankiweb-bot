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
            Console.WriteLine("Login: Login has been called");

            string username, password;
            string csrfToken, sessionToken;

            (username, password) = await AskForCredentials(user);
            (csrfToken, sessionToken) = await user.Login(username, password);

            Console.WriteLine("csrfToken   : {0}", csrfToken);
            Console.WriteLine("sessionToken: {0}", sessionToken);

            user.CsrfToken = csrfToken;
            user.Cookie = sessionToken;

            _controller.UpdateUser(user);
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

                case Command.SearchCards:
                    return AskForCredentials(user);

                default:
                    throw new NotImplementedException();
            }

        }
    }
}