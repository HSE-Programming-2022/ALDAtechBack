using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

// var bot = new TelegramBot("5001678276:AAH0MgT6aPNR7pkOGeD5eYFxEsJI3sgE4WA");
// Console.ReadLine();


namespace AldaTech_api.BotCore;




public class BotClientTelegram : IBotClient
{
    private Telegram.Bot.TelegramBotClient _botClient;
    private string _token;
    private CancellationTokenSource _cts;
    private ReceiverOptions _receiverOptions;
    private List<MessageListener> _listeners;

    public BotClientTelegram(string token)
    {
        _token = token;
        _botClient = new Telegram.Bot.TelegramBotClient(token);
        _cts = new CancellationTokenSource();
        
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

    public void Destroy()
    {
        _cts.Cancel();
    }
    
    public TaskCompletionSource<Message> WaitFor(Func<Message, bool> check)
    {
        var listener = new MessageListener(check);
        _listeners.Add(listener);
        return listener.Task;
    }
    
    async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
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
        
        var bot = new BotManager(this, message);
        var task = bot.Run();
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

    public async Task<Message> GetUserMessageAsync(long chatId)
    {
        var msg = await (WaitFor(x => x.Chat.Id == chatId).Task);
        return msg;
    }
    
    // private async Task NewDialog(Message message)
    // {
    //     var chatId = message.Chat.Id;
    //     var messageText = message.Text;
    //
    //     Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
    //
    //     // Echo received message text
    //     await _botClient.SendTextMessageAsync(
    //         chatId: chatId,
    //         text: "Ener age",
    //         cancellationToken: _cts.Token);
    //     var msg = await (WaitFor(x => x.Chat.Id == chatId).Task);
    //     var age = msg.Text;
    //     await _botClient.SendTextMessageAsync(
    //         chatId: chatId,
    //         text: "Enter name",
    //         cancellationToken: _cts.Token);
    //     msg = await (WaitFor(x => x.Chat.Id == chatId).Task);
    //     var name = msg.Text;
    //     
    //     await _botClient.SendTextMessageAsync(
    //         chatId: chatId,
    //         text: age + " - " + name,
    //         cancellationToken: _cts.Token);
    // }
}
