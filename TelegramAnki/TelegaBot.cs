using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
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
        TelegramBotClient _client;

        public TelegaBot(string token)
        {
            this._token = token;
            this._client = new TelegramBotClient(_token);
        }
        internal void GetUpdates()
        {
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
                case UpdateType.Message:
                    Message? message = update.Message;
                    if (message == null)
                    {
                        break;
                    }
                    _client.SendTextMessageAsync(message.Chat.Id, "Receive text:" + message.Text, replyMarkup: GetButtons());
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
