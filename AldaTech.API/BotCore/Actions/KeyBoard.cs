using AldaTech_api.BotFactory;
using Newtonsoft.Json;
using Telegram.Bot.Types.ReplyMarkups;

namespace AldaTech_api.BotCore;

public class KeyBoard : IBotAction
{
    private IBotClient _botClient;
    private long _chatId;

    [JsonProperty]
    public List<string> Options;
    
    [JsonProperty]
    public string Text;

    public KeyBoard(BotComponentData keyboardData)
    {
        Text = keyboardData.Text;
        Options = new List<string>();
        foreach (var buttonData in keyboardData.Children)
        {
            if (buttonData.Type == "Button") 
            {
                Console.WriteLine(buttonData.ButtonText);
                Options.Add(buttonData.ButtonText);
            }
        }
    }
    public KeyBoard(string text, List<string> options, IBotClient botClient, long chatId)
    {
        Text = text;
        Options = options;
        _botClient = botClient;
        _chatId = chatId;
    }
    
    
    
    public async Task<ActionExecutionResult> Run(BotUserContext ctx)
    {
        var keyboard = new KeyboardButton[Options.Count];
        for (int i = 0; i < keyboard.Length; i++)
        {
            keyboard[i] = Options[i];
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
        
        await _botClient.SendKeyBoardAsync(_chatId, Text, keys);
        return new ActionExecutionResult(ActionExecutionStatus.RunNext);
    }
}