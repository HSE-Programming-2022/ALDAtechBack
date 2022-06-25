using System.Security.Cryptography.X509Certificates;
using AldaTech_api.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

// var bot = new TelegramBot("5001678276:AAH0MgT6aPNR7pkOGeD5eYFxEsJI3sgE4WA");
// Console.ReadLine();


namespace AldaTech_api.BotCore;

public class BotClientTelegram : IBotClient
{
    private Telegram.Bot.TelegramBotClient _botClient;
    private Bot _botInfo;
    private string _token;
    private List<Task> _user;
    private CancellationTokenSource _cts;
    private ReceiverOptions _receiverOptions;
    private List<MessageListener> _listeners;
    private List<BotManager> _botManagers;
    private string _botManagerPath = "./Data/bot.json";
    public int Id { get; private set; }
    public BotClientTelegram(Bot botInfo)
    {
        
        _botInfo = botInfo;
        _token = botInfo.Token;
        _botManagerPath = botInfo.BotManagerPath;
        Id = botInfo.Id;
        
        _botClient = new Telegram.Bot.TelegramBotClient(_token);
        _cts = new CancellationTokenSource();
        _botManagers = new List<BotManager>();

        _receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
        };

        _botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: _receiverOptions,
            cancellationToken: _cts.Token
        );

        _listeners = new List<MessageListener>();
    }


    public void Stop()
    {
        // throw new TaskCanceledException();
        Console.WriteLine("Stoping bot");
        _cts.Cancel();
        Console.WriteLine(_cts.Token.IsCancellationRequested);
        while (_listeners.Count != 0)
        {
            var listener = _listeners[0];
            listener.Task.SetResult(null);
            _listeners.Remove(listener);
        }
        
        Console.WriteLine(_listeners.Count);
        Console.WriteLine(_cts.Token.IsCancellationRequested);
    }
    
    public TaskCompletionSource<Message> WaitFor(Func<Message, bool> check)
    {
        var listener = new MessageListener(check);
        _listeners.Add(listener);
        return listener.Task;
    }
    
    async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        Console.WriteLine(_cts.Token == cancellationToken);
        Console.WriteLine("Handle updates " + _cts.IsCancellationRequested);
        Console.WriteLine("Handle updates " + cancellationToken.IsCancellationRequested);
        Console.WriteLine("Handle updates " + _cts.Token.IsCancellationRequested);
        Console.WriteLine("Handle updates " + _listeners.Count);
        // Only process Message updates: https://core.telegram.org/bots/api#message
        if (update.Type != UpdateType.Message)
            return;
        if (update.Message is null)
            return;
        // Only process text messages
        if (update.Message!.Type != MessageType.Text)
            return;

        Message message = update.Message;
        Console.WriteLine("Got message" + message.Text);
        foreach (var listener in _listeners)
        {
            if (listener.ListensTo(message))
            {
                listener.Task.TrySetResult(message);
                _listeners.Remove(listener);
                Console.WriteLine("Listener worked");
                return;
            }
        }

        var bot = BotJsonStorage.ReadBotManager(_botManagerPath);
        BotUserContext ctx = new BotUserContext(){BotClient = this, ChatId = message.Chat.Id, Ct = _cts.Token};
        bot.Run(ctx);
        _botManagers.Add(bot);
    }
    
    Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }

    public async Task SendTextMessageAsync(long chatId, string text)
    {
        await _botClient.SendTextMessageAsync(
            chatId: chatId,
            text: text
            );
    }
    
    public async Task SendKeyBoardAsync(long chatId, string text, ReplyKeyboardMarkup keys)
    {
        await _botClient.SendTextMessageAsync(
            chatId: chatId,
            text: text,
            replyMarkup: keys
        );
    }

    public async Task<Message> GetUserMessageAsync(long chatId)
    {
        var msg = await (WaitFor(x => x.Chat.Id == chatId).Task);
        return msg;
    }
}
