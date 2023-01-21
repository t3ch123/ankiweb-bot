using System.Text.Json.Serialization;

namespace AnkiWeb
{
    public class User
    {
        [JsonPropertyName("login")]
        public string Login;
        [JsonPropertyName("password")]
        public string Password;

        public User(string login, string password)
        {
            Login = login;
            Password = password;
        }

    }

    class Deck
    {

    }

    class Card
    {

    }
}