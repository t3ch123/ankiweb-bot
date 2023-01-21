using Anki.IL;
using Microsoft.Extensions.Configuration;
using TelegramAnki.Settings;

namespace Main
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            Settings settings = config.GetRequiredSection("Settings").Get<Settings>();

            Console.WriteLine("PgConnectionString {0}", settings.PgConnectionString);
            Console.WriteLine("TgSecretToken {0}", settings.TgSecretToken);

            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                TelegaBot telegaBot = new(settings: settings);
                telegaBot.GetUpdates();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
