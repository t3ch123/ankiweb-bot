using Anki.BLL;
using TelegramAnki.User;

namespace StateMachine
{
    public enum BotState
    {
        Initial,
        WaitingForLogin,
        LoggedIn
    }

    public enum Command
    {
        Start,
        Login,
        ViewDecks,
        SearchCards
    }

    public class Bot
    {
        class StateTransition
        {
            readonly BotState CurrentState;
            readonly Command Command;

            public StateTransition(BotState currentState, Command command)
            {
                CurrentState = currentState;
                Command = command;
            }

            public override int GetHashCode() => 17 + 31 * CurrentState.GetHashCode() + 31 * Command.GetHashCode();

            public override bool Equals(Object? obj)
            {
                if (obj is StateTransition other)
                {
                    return CurrentState == other.CurrentState && Command == other.Command;
                }
                return false;
            }
        }

        private readonly Dictionary<StateTransition, BotState> transitions;
        public BotState CurrentState { get; private set; }

        private readonly Controller controller;

        public Bot(Controller controller)
        {
            CurrentState = BotState.Initial;
            transitions = new Dictionary<StateTransition, BotState>
            {
                { new StateTransition(BotState.Initial, Command.Start), BotState.WaitingForLogin },
                { new StateTransition(BotState.WaitingForLogin, Command.Login), BotState.LoggedIn },
                { new StateTransition(BotState.LoggedIn, Command.ViewDecks), BotState.LoggedIn },
                { new StateTransition(BotState.LoggedIn, Command.SearchCards), BotState.LoggedIn },
                { new StateTransition(BotState.LoggedIn, Command.Start), BotState.Initial },
            };
            this.controller = controller;
        }
        public BotState GetNext(long ChatID, Command command)
        {
            TelegramUser user = controller.GetUser(ChatID);
            StateTransition stateTransition = new(
                currentState: (BotState)user.State,
                command: command
            );
            CurrentState = transitions[stateTransition];
            return CurrentState;
        }

        public BotState MoveNext(long ChatID, Command command)
        {
            TelegramUser user = controller.GetUser(ChatID);
            StateTransition stateTransition = new(
                currentState: (BotState)user.State,
                command: command
            );
            CurrentState = transitions[stateTransition];
            user.State = (int)CurrentState;
            controller.UpdateUser(user);
            return CurrentState;
        }

        public static Command? ConvertStringToCommand(string? text)
        {
            return text switch
            {
                "/start" => (Command?)Command.Start,
                "/login" => (Command?)Command.Login,
                "/decks" => (Command?)Command.ViewDecks,
                "/search-cards" => (Command?)Command.SearchCards,
                null => null,
                _ => null,
            };
        }
    }
}