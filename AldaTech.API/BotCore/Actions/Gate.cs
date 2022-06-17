using Telegram.Bot.Types;

namespace AldaTech_api.BotCore;

public class Option
{
    public string TextOption {get; set; }
    public IBotAction Action {get; set; }

    public Option(string textOption, IBotAction action)
    {
        TextOption = textOption;
        Action = action;
    }

    public bool IsCorrect(string q) => string.Equals(q, TextOption, StringComparison.CurrentCultureIgnoreCase);

}


public class Gate : IBotAction
{
    private IBotClient _botClient;
    private long _chatId;
    public  List<Option> _options;

    public Gate(IBotClient botClient, long chatId, List<Option> options)
    {
        _botClient = botClient;
        _chatId = chatId;
        _options = options;
    }

    public async Task<ActionExecutionResult> Run()
    {
        var msg = await _botClient.GetUserMessageAsync(_chatId);
        Console.WriteLine("GATE GOT MESSAGE " + msg);
        foreach (var option in _options)
        {
            if (!option.IsCorrect(msg.Text))
                continue;
            var action = option.Action;
            Console.WriteLine(action.GetType());
            // Допустимы - отправка сообщений + Переходы на другие экраны
            if (action.GetType() == typeof(TextMessage))
            {
                await action.Run();
                return new ActionExecutionResult(ActionExecutionStatus.RunNext);    
            }

            if (action.GetType() == typeof(Redirect))
            {
                return new ActionExecutionResult(ActionExecutionStatus.SwitchWindow, action as Redirect);    
            }
            
            // return new ActionExecutionResult(ActionExecutionStatus.RunNext);
        }
        _botClient.SendTextMessageAsync(_chatId, "Нормальное разговаривай: ваш ответ не соотвествует ни одному варианту");
        return new ActionExecutionResult(ActionExecutionStatus.RunNext);
    }
}