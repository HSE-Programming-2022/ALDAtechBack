using Newtonsoft.Json;
using Telegram.Bot.Types.ReplyMarkups;

namespace AldaTech_api.BotCore;

public class KeyBoard : IBotAction
{
    private IBotClient _botClient;
    [JsonProperty]
    private ReplyKeyboardMarkup _keys;
    [JsonProperty]
    private string _text;
    private long _chatId;
    
    public KeyBoard(string text, ReplyKeyboardMarkup keys, IBotClient botClient, long chatId)
    {
        _text = text;
        _keys = keys;
        _botClient = botClient;
        _chatId = chatId;
    }
    
    public async Task<ActionExecutionResult> Run(BotUserContext ctx)
    {
        _botClient = ctx.BotClient;
        _chatId = ctx.ChatId;
        
        await _botClient.SendKeyBoardAsync(_chatId, _text, _keys);
        return new ActionExecutionResult(ActionExecutionStatus.RunNext);
    }
}