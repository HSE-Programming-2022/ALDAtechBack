using Newtonsoft.Json;

namespace AldaTech_api.BotCore;

public class TextMessage : IBotAction
{
    private IBotClient _botClient;
    private long _chatId;

    [JsonProperty]
    public string Text;
    
    public TextMessage(string text, IBotClient botClient, long chatId)
    {
        Text = text;
        _botClient = botClient;
        _chatId = chatId;
    }
    public async Task<ActionExecutionResult> Run(BotUserContext ctx)
    {
        _botClient = ctx.BotClient;
        _chatId = ctx.ChatId;

        Console.WriteLine("Sending " + _chatId + " " + Text);
        await _botClient.SendTextMessageAsync(_chatId, Text);
        return new ActionExecutionResult(ActionExecutionStatus.RunNext);
    }
}
