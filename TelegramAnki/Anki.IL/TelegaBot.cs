﻿using Anki.BLL;
using StateMachine;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramAnki.Settings;
using TelegramAnki.User;

namespace Anki.IL
{
    public class TelegaBot
    {
        private const string text1 = "Again";
        private const string text2 = "Hard";
        private const string text3 = "Good";
        private const string text4 = "Easy";


        private readonly string _token;
        readonly TelegramBotClient _client;
        readonly Controller _controller;
        readonly ICommandsRepository _handler;
        readonly Bot _bot;

        public TelegaBot(Settings settings)
        {
            _token = settings.TgSecretToken;
            _client = new TelegramBotClient(_token);
            _controller = new Controller(settings: settings);
            _handler = new CommandsRepository(controller: _controller);
            _bot = new Bot(controller: _controller);
        }
        public async void GetUpdates()
        {
            var me = _client.GetMeAsync().Result;
            if (me != null && !string.IsNullOrEmpty(me.Username))
            {
                int offset = 0;
                while (true)
                {
                    var updates = _client.GetUpdatesAsync(offset).Result;
                    if (updates != null && updates.Length > 0)
                    {
                        foreach (var update in updates)
                        {
                            try
                            {
                                ProcessUpdate(update).Wait();
                            }
                            catch (AggregateException aex)
                            {
                                aex.Handle(ex =>
                                {
                                    Console.WriteLine("GetUpdates: {0} {1}", ex.GetType().Name, ex.Message);
                                    return true;
                                });
                            }
                            finally
                            {
                                offset = update.Id + 1;
                            }
                        }
                    }

                    Thread.Sleep(1000);
                }
            }
        }
        private async Task ProcessUpdate(Update update)
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                    Message? message = update.Message;

                    if (message == null)
                    {
                        break;
                    }

                    TelegramUser user;

                    if (!_controller.UserExists(ChatID: message.Chat.Id))
                    {
                        Console.WriteLine("ProcessUpdate: New user joined {0}", message.Chat.Id);

                        user = new()
                        {
                            ChatID = message.Chat.Id,
                            State = (int)BotState.Initial
                        };

                        Console.WriteLine(
                            "ProcessUpdate: Create an entry for user {0}, state = {1}",
                            user.ChatID,
                            ((BotState)user.State).ToString()
                        );
                        _controller.CreateUser(user);
                    }

                    user = _controller.GetUser(ChatID: message.Chat.Id);

                    Command cmd = Bot.ConvertStringToCommand(message.Text);
                    switch (cmd)
                    {
                        case Command.Start:
                            Console.WriteLine("ProcessUpdate: next state {0}", _bot.GetNext(user.ChatID, cmd));
                            _bot.MoveNext(user.ChatID, cmd);
                            await _client.SendTextMessageAsync(message.Chat.Id, "Received Start command", replyMarkup: Buttons);
                            break;
                        case Command.Login:
                            await _client.SendTextMessageAsync(message.Chat.Id, "Received Login command", replyMarkup: Buttons);
                            Console.WriteLine("ProcessUpdate: next state {0}", _bot.GetNext(user.ChatID, cmd));

                            _handler.ExecuteCommand(cmd: cmd, user: user).Wait();
                            _bot.MoveNext(user.ChatID, cmd);
                            break;
                        case Command.ViewDecks:
                            _bot.MoveNext(user.ChatID, Command.ViewDecks);
                            await _client.SendTextMessageAsync(message.Chat.Id, "Received ViewDecks command", replyMarkup: Buttons);
                            break;
                        case Command.SearchCards:
                            _bot.MoveNext(user.ChatID, Command.SearchCards);
                            await _client.SendTextMessageAsync(message.Chat.Id, "Received SearchCards command", replyMarkup: Buttons);
                            break;
                        default:
                            await _client.SendTextMessageAsync(message.Chat.Id, "Received Unknown command", replyMarkup: Buttons);
                            break;
                    }
                    break;

                default:
                    Console.WriteLine("ProcessUpdate: command '{0}' is not implemented", update.Type);
                    break;
            }
        }
        private static IReplyMarkup Buttons
        {
            get
            {
                ReplyKeyboardMarkup rkm = new(new[]
                {
                new[]
                {
                    new KeyboardButton(text1),
                    new KeyboardButton(text2),
                    new KeyboardButton(text3),
                    new KeyboardButton(text4),
                }
            })
                {
                    ResizeKeyboard = true
                };
                return rkm;
            }
        }
    }
}
