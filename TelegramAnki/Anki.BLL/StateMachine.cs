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
        SearchCards,
        Defunct
    }


    public class StateTransition
    {
        public readonly Command Command;
        public BotState CurrentState { get; private set; }

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

    public class Bot
    {

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

            if (!transitions.ContainsKey(stateTransition))
                throw new InvalidTransition(stateTransition);

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

            if (!transitions.ContainsKey(stateTransition))
                throw new InvalidTransition(stateTransition);

            CurrentState = transitions[stateTransition];

            user.State = (int)CurrentState;
            controller.UpdateUser(user);
            return CurrentState;
        }

        public static Command ConvertStringToCommand(string? text)
        {
            return text switch
            {
                Commands.startCmdStr        => Command.Start,
                Commands.loginCmdStr        => Command.Login,
                Commands.showDesksCmdStr    => Command.ViewDecks,
                Commands.searchNoteCmdStr   => Command.SearchCards,
                null => Command.Defunct,
                _ => Command.Defunct,
            };
        }
    }
}