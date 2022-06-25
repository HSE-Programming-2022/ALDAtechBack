using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace AldaTech_api.BotCore;

public interface IBotClient
{
    public Task SendTextMessageAsync(long chatId, string text);

    public Task SendKeyBoardAsync(long chatId, string text, ReplyKeyboardMarkup keys);
    
    public Task<Message> GetUserMessageAsync(long chatId);
}