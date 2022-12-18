
using System.Text.RegularExpressions;

namespace AnkiWeb
{

    public class AnkiWebAPI
    {

        private static class Endpoints
        {
            public const string
                Login = "account/login",
                Decks = "";

            public const string
                Domain = "ankiweb.net";

            public static string GetUrl(string endpoint)
            {
                return string.Format("https://{0}/{1}", Domain, endpoint);
            }
        }

        private static readonly HttpClient _client = new();


        public async Task<(string, string)> Login(string username, string password)
        {
            string csrfToken;
            string sessionToken;

            HttpResponseMessage response;
            string loginUrl = Endpoints.GetUrl(Endpoints.Login);

            string pattern = @"csrf_token"" value=""(.+)""";
            Regex r = new(pattern, RegexOptions.IgnoreCase);

            response = await _client.GetAsync(loginUrl);

            Task<string> stringContentsTask = response.Content.ReadAsStringAsync();
            string? responseContent = stringContentsTask.Result;

            Console.WriteLine("Response: " + response);
            Console.WriteLine("Response content: " + responseContent);

            if (responseContent == null)
            {
                Console.WriteLine("Failed to retrieve CSRF, got null response");
                return ("", "");
            }

            Match m = r.Match(input: responseContent);

            if (m.Groups.Count != 2)
            {
                return ("", "");
            }
            csrfToken = m.Groups[1].Value;

            // var content = new FormUrlEncodedContent(
            //     new Dictionary<string, string> {
            //         {"username", username},
            //         {"password", password},
            //     }
            // );
            // response = await _client.PostAsync(loginUrl, content);

            sessionToken = "";
            return (csrfToken, sessionToken);
        }
    }
}