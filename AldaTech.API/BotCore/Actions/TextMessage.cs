using Newtonsoft.Json;

namespace AldaTech_api.BotCore;

public class TextMessage : IBotAction
{
    private IBotClient _botClient;
    [JsonProperty]
    private string _text;
    private long _chatId;

    public TextMessage(string text, IBotClient botClient, long chatId)
    {
        _text = text;
        _botClient = botClient;
        _chatId = chatId;
    }
    public async Task<ActionExecutionResult> Run(BotUserContext ctx)
    {
        _botClient = ctx.BotClient;
        _chatId = ctx.ChatId;

        Console.WriteLine("Sending " + _chatId + " " + _text);
        await _botClient.SendTextMessageAsync(_chatId, _text);
        return new ActionExecutionResult(ActionExecutionStatus.RunNext);
    }
}
