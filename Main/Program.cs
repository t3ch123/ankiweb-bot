using Anki.IL;

namespace Main
{
    class Program
    {
        const string SECRET_TOKEN = "5339915288:AAFtdLwrYGHY7U6swyciwYUO5gODVG6FPwk";
        static void Main(string[] args)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                TelegaBot telegaBot = new(token: SECRET_TOKEN);
                telegaBot.GetUpdates();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
