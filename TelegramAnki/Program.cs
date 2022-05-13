using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramAnki
{
    class Program
    {
        const string SECRET_TOKEN = "5339915288:AAFtdLwrYGHY7U6swyciwYUO5gODVG6FPwk";
        static void Main(string[] args)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                TelegaBot telegaBot = new TelegaBot(token: SECRET_TOKEN);
                telegaBot.GetUpdates();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}