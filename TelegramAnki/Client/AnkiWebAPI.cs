
using System.Net;
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

        private readonly HttpClient _client;
        private readonly HttpClientHandler _handler;


        public AnkiWebAPI()
        {
            _handler = new HttpClientHandler
            {
                CookieContainer = new CookieContainer(),
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
                AllowAutoRedirect = false,
                Proxy = new WebProxy
                {
                    Address = new Uri($"http://localhost:1337"),
                    BypassProxyOnLocal = false,
                    UseDefaultCredentials = false,
                }
            };
            _client = new HttpClient(handler: _handler);
        }

        private static Cookie CreateCookie(string cookieString)
        {
            Console.WriteLine("Cookie string: {0}", cookieString);
            var properties = cookieString.Split(';', StringSplitOptions.TrimEntries).ToList();
            Console.WriteLine("Properties: {0}", properties);
            var name = properties[0].Split("=")[0];
            var value = properties[0].Split("=")[1];
            var path = properties[2].Replace("path=", "");
            var cookie = new Cookie(name, value, path)
            {
                Secure = properties.Contains("secure"),
                HttpOnly = properties.Contains("httponly"),
                // Expires = DateTime.Parse(properties[1].Replace("expires=", ""))
            };
            return cookie;
        }

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

            // Console.WriteLine("Response: " + response);
            // Console.WriteLine("Response content: " + responseContent);

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

            var content = new FormUrlEncodedContent(
                new Dictionary<string, string> {
                    {"username", "lyamets.misha@gmail.com"},
                    {"password", ""},
                    {"csrf_token", csrfToken},
                    {"submitted", "1"},
                }
            );
            _handler.CookieContainer.Add(
                new Cookie(name: "ankiweb", value: "login")
                {
                    Domain = "ankiweb.net",
                    Path = "/",
                }
            );
            response = await _client.PostAsync(loginUrl, content);

            stringContentsTask = response.Content.ReadAsStringAsync();
            responseContent = stringContentsTask.Result;

            if (response.StatusCode != HttpStatusCode.Redirect)
            {
                throw new Exception("Unexpected status code on login: " + response.StatusCode.ToString());
            }

            Console.WriteLine("LoginUrl {0}", loginUrl);
            Console.WriteLine("Response: " + response);
            Console.WriteLine("Response content: " + responseContent);

            foreach (var item in _handler.CookieContainer.GetAllCookies())
            {
                Console.WriteLine("Cookie: " + item.ToString());
            }

            response.Headers.TryGetValues("Set-Cookie", out var cookiesHeader);

            if (cookiesHeader != null)
            {
                List<Cookie> cookies = cookiesHeader.Select(cookieString => CreateCookie(cookieString)).ToList();
                Cookie? sessionId = cookies.Where(x => x.Name == "ankiweb").First();
                sessionToken = sessionId.Value;
            }
            else
            {
                sessionToken = "";
            }

            return (csrfToken, sessionToken);
        }
    }
}