namespace StateMachine 
{
    public class InvalidTransition : Exception
    {
        public InvalidTransition(StateTransition stateTransition) : base(stateTransition.CurrentState.ToString() + " x " + stateTransition.Command.ToString()) { }
    }
}