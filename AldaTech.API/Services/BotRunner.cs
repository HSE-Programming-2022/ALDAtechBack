using AldaTech_api.Models;
using AldaTech_api.Services;

namespace AldaTech_api.BotCore;

//  Класс отвечающий за запуск и отстановку ботов
// Запускает и останавливает по id бота.


public class BotRunner : IBotRunner
{
    private readonly IServiceScopeFactory _scopeFactory;
    
    private List<BotClientTelegram> _botClients;
    
    public BotRunner(IServiceScopeFactory scopeFactory)
    {
        _botClients = new List<BotClientTelegram>();
        _scopeFactory = scopeFactory;
        
        // Делает первого тестового бота в бд 
        // using (var scope = _scopeFactory.CreateScope())
        // {
        //     var botToSave = new Bot()
        //     {
        //         BotManagerPath = "./Data/1.json", 
        //         Id = 1, 
        //         Title = "Тестовый бот",
        //         IsRunning = false,
        //         Token = "5001678276:AAH0MgT6aPNR7pkOGeD5eYFxEsJI3sgE4WA"
        //             
        //     };
        //     var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        //     context.Add(botToSave);
        //     context.SaveChanges();
        // }

    }

    public async Task<string> StartBot(int botId)
    {
        // var botInfo = new Bot()
        // {
        //     BotManagerPath = "./Data/1.json", 
        //     Id = 1, 
        //     Title = "Тестовый бот",
        //     IsRunning = false,
        //     Token = "5001678276:AAH0MgT6aPNR7pkOGeD5eYFxEsJI3sgE4WA"
        //         
        // };
        // var bot = new BotClientTelegram(botInfo);
        // _botClients.Add(bot);
        // return "Bot started";
        
        using(var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            var botInfo = context.Bots.FirstOrDefault(x => x.Id == botId);
            if (botInfo is null)
                return "No such bot with id " + botId; 
        
            if (botInfo.IsRunning)
                return "Already running";
        
            var bot = new BotClientTelegram(botInfo);
            _botClients.Add(bot);
            botInfo.IsRunning = true;
            context.SaveChanges();
            return "Bot started";
        }
    }

    public async Task<string> StopBot(int botId)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        
            var botInfo = context.Bots.FirstOrDefault(x => x.Id == botId);
            if (botInfo is null)
                return "No such bot with id " + botId;
        
            if (!botInfo.IsRunning)
                return "Bot is not running";
        
            var bot = _botClients.FirstOrDefault(bc => bc.Id == botId);
            if (bot is null)
                return "No such bot running";
        
            bot.Stop();
            botInfo.IsRunning = false;
            await context.SaveChangesAsync();
            return "Bot stopped";
        }
        

        // var bot = _botClients.FirstOrDefault(bc => bc.Id == botId);
        // if (bot is null)
        //     return "No such bot running";
        //
        // bot.Stop();
        // _botClients.Remove(bot);
        // return "Bot stopped";
    }
    
    public async Task<string> RestartBot(int botId)
    {
        await StopBot(botId);
        return await StartBot(botId);
    }
}