using Telegram.Bot.Types;

namespace AldaTech_api.BotCore;

public class BotManager
{
    private IBotClient _botClient;
    private List<Screen> _screens;
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
        // actions.Add(new TextMessage("Тест 3", _botClient, _chatId));
        
        _screens.Add(new Screen(_botClient, _chatId, actions, 1));
        actions = new List<IBotAction>();
        actions.Add(new TextMessage("Это экран 2", _botClient, _chatId));
        _screens.Add(new Screen(_botClient, _chatId, actions, 2));
    }
    
    public BotManager(IBotClient botClient, Message message)
    {
        _botClient = botClient;
        _screens = new List<Screen>();
        _chatId = message.Chat.Id;
        
        // TODO инициализация из JSON
        
        SetDefaultBot();
    }

    public async Task Run()
    {
        var screen = _screens.FirstOrDefault();
        while (true)
        {
            var redirect = await screen.Run();

            if (redirect is null)
            {
                await _botClient.SendTextMessageAsync(_chatId, "На этом все! Пишите еще");
                Console.WriteLine("No redirection - Exiting user");
                return;
            }

            screen = _screens.FirstOrDefault(x => x.Id == redirect.ScreenId);
        }
    }

}