namespace AnkiWeb
{
    public class LoginFailure : Exception
    {
        public LoginFailure(string message) : base(message) { }
    }
}