using Newtonsoft.Json;
using Telegram.Bot.Types.ReplyMarkups;

namespace AldaTech_api.BotCore;

public class KeyBoard : IBotAction
{
    private IBotClient _botClient;
    [JsonProperty]
    private List<string> _options;
    [JsonProperty]
    private string _text;
    private long _chatId;
    
    public KeyBoard(string text, List<string> options, IBotClient botClient, long chatId)
    {
        _text = text;
        _options = options;
        _botClient = botClient;
        _chatId = chatId;
    }
    
    
    
    public async Task<ActionExecutionResult> Run(BotUserContext ctx)
    {
        var keyboard = new KeyboardButton[_options.Count];
        for (int i = 0; i < keyboard.Length; i++)
        {
            keyboard[i] = _options[i];
        }
        
        ReplyKeyboardMarkup keys = new(new []
        {
            keyboard ,
        })
        {
            ResizeKeyboard = true
        };
        
        _botClient = ctx.BotClient;
        _chatId = ctx.ChatId;
        
        await _botClient.SendKeyBoardAsync(_chatId, _text, keys);
        return new ActionExecutionResult(ActionExecutionStatus.RunNext);
    }
}