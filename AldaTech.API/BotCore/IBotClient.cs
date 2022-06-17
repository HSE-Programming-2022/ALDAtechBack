using Telegram.Bot.Types;

namespace AldaTech_api.BotCore;

public interface IBotClient
{
    public Task SendTextMessageAsync(long chatId, string text);
    
    public Task<Message> GetUserMessageAsync(long chatId);
}