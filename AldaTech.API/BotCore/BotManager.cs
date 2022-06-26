using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace AldaTech_api.BotCore;

public class BotUserContext
{
    public IBotClient BotClient {get; set; }
    
    public CancellationToken Ct { get; set; }
    public long ChatId {get; set; }
}

public class BotManager
{
    private const string BotManagerPath = "./Data/bot.json";
    private IBotClient _botClient;
    
    [JsonProperty]
    private List<Screen> Screens;
    
    
    private long _chatId;

    private void SetDefaultBot()
    {
        var actions = new List<IBotAction>();
        
        actions.Add(new TextMessage("Сколько будет 2 * 2?", _botClient, _chatId));
        
        
        var options = new List<Option>();
        options.Add(new Option("4", new TextMessage("Жесть ты умный", _botClient, _chatId)));
        options.Add(new Option("2", new TextMessage("Дурак?", _botClient, _chatId)));


        actions.Add(new Gate(_botClient, _chatId, options));
        actions.Add(new TextMessage("Абоба", _botClient, _chatId));
        actions.Add(new Redirect(2));
        var keyboardOptions = new List<string>();
        keyboardOptions.Add("Я");
        keyboardOptions.Add("Не Я");
        keyboardOptions.Add("кто?");
        actions.Add(new KeyBoard("кто ты?", keyboardOptions, _botClient, _chatId));
        // actions.Add(new TextMessage("Тест 3", _botClient, _chatId));
        
        Screens.Add(new Screen(_botClient, _chatId, actions, 1));
        actions = new List<IBotAction>();
        actions.Add(new TextMessage("Это экран 2", _botClient, _chatId));
        Screens.Add(new Screen(_botClient, _chatId, actions, 2));
    }

    
    public BotManager()
    {
        Screens = new List<Screen>();
        SetDefaultBot();
        BotJsonStorage.SaveBotManager("./Data/kb.json", this);
    }
    
    // public BotManager()
    // {
    //     Screens = new List<Screen>();
    //     // SetDefaultBot();
    //     // BotJsonStorage.SaveBotManager("./Data/kb.json", this);
    // }

    public async Task Run(BotUserContext ctx)
    {
        _botClient = ctx.BotClient;
        _chatId = ctx.ChatId;


        var screen = Screens.FirstOrDefault();
        while (screen is not null)
        {
            if (ctx.Ct.IsCancellationRequested)
                return;

            var executionResult = await screen.Run(ctx);

            Console.WriteLine("Bot manager " + executionResult.Status);
            switch (executionResult.Status)
            {
                
                case ActionExecutionStatus.Cancelled:
                    return;
                    break;
                case ActionExecutionStatus.Error:
                    return;
                    break;
                case ActionExecutionStatus.SwitchWindow:
                    var redirect = executionResult.Redirect;
                    screen = Screens.FirstOrDefault(x => x.Id == redirect.ScreenId);
                    break;
                case ActionExecutionStatus.Done:
                    await _botClient.SendTextMessageAsync(_chatId, "На этом все! Пишите еще");
                    Console.WriteLine("No redirection - Exiting user");
                    return;
            }
        }

        
    }

}