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

            public override int GetHashCode()
            {
                return 17 + 31 * CurrentState.GetHashCode() + 31 * Command.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                StateTransition other = obj as StateTransition;
                return other != null && this.CurrentState == other.CurrentState && this.Command == other.Command;
            }
        }

        readonly Dictionary<StateTransition, BotState> transitions;
        public BotState CurrentState { get; private set; }

        public Bot()
        {
            CurrentState = BotState.Initial;
            transitions = new Dictionary<StateTransition, BotState>
            {
                { new StateTransition(BotState.Initial, Command.Start), BotState.WaitingForLogin },
                { new StateTransition(BotState.WaitingForLogin, Command.Login), BotState.LoggedIn },
                { new StateTransition(BotState.LoggedIn, Command.ViewDecks), BotState.LoggedIn },
                { new StateTransition(BotState.LoggedIn, Command.SearchCards), BotState.LoggedIn },
            };
        }

        public BotState GetNext(Command command)
        {
            StateTransition transition = new(CurrentState, command);
            if (!transitions.TryGetValue(transition, out BotState nextState))
                throw new Exception("Invalid transition: " + CurrentState + " -> " + command);
            return nextState;
        }

        public BotState MoveNext(Command command)
        {
            CurrentState = GetNext(command);
            return CurrentState;
        }
    }
}