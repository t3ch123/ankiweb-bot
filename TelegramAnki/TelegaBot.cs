using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramAnki
{
    internal class TelegaBot
    {
        private const string text1 = "Again";
        private const string text2 = "Hard";
        private const string text3 = "Good";
        private const string text4 = "Easy";
        private string _token;
        Telegram.Bot.TelegramBotClient _client;

        public TelegaBot(string token)
        {
            this._token = token;
        }
        internal void GetUpdates()
        {
            _client = new Telegram.Bot.TelegramBotClient(_token);
            var me = _client.GetMeAsync().Result;
            if (me != null && !string.IsNullOrEmpty(me.Username))
            {
                int offset = 0;
                while (true)
                {
                    try
                    {
                        var updates = _client.GetUpdatesAsync(offset).Result;
                        if (updates != null && updates.Count() > 0)
                        {
                            foreach (var update in updates)
                            {
                                processUpdate(update);
                                offset = update.Id + 1;
                            }
                        }
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }

                    Thread.Sleep(1000);
                }
            }
        }
        private void processUpdate(Telegram.Bot.Types.Update update)
        {
            switch (update.Type)
            {
                case Telegram.Bot.Types.Enums.UpdateType.Message:
                    var text = update.Message.Text;
                    _client.SendTextMessageAsync(update.Message.Chat.Id, "Receive text:" + text, replyMarkup: GetButtons());
                    break;
                default:
                    Console.WriteLine(update.Type + "Not implemented");
                    break;
            }
        }
        private IReplyMarkup GetButtons()
        {
            ReplyKeyboardMarkup rkm = new ReplyKeyboardMarkup(new[]
            {
                new[]
                {
                    new KeyboardButton(text1),
                    new KeyboardButton(text2),
                    new KeyboardButton(text3),
                    new KeyboardButton(text4),
                }
            });
            rkm.ResizeKeyboard = true;
            return rkm;
        }
    }
}
