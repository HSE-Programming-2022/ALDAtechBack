using AldaTech_api.BotFactory;
using JsonSubTypes;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace AldaTech_api.BotCore;



public class Gate : IBotAction
{
    private IBotClient _botClient;
    private long _chatId;
    
    public List<GateOption> Options;
    public IBotAction? DefaultAction { get; set; }

    public Gate(IBotClient botClient, long chatId, List<GateOption> options)
    {
        _botClient = botClient;
        _chatId = chatId;
        Options = options;
    }

    public Gate(BotComponentData botComponentData)
    {
        Options = new List<GateOption>();
        foreach (var optionData in botComponentData.Children)
        {
            GateOption gateOption;
            if (optionData.Type == "GateOption")
            {
                gateOption = new GateOption(optionData);
                Options.Add(gateOption);
            }
        }
    }
    
    public async Task<ActionExecutionResult> Run(BotUserContext ctx)
    {
        _chatId = ctx.ChatId;
        _botClient = ctx.BotClient;

        var msg = await _botClient.GetUserMessageAsync(_chatId);
        Console.WriteLine("Msg" + msg);
        switch (msg)
        {
            case null when ctx.Ct.IsCancellationRequested:
                Console.WriteLine("Gate cancelled");
                return new ActionExecutionResult(ActionExecutionStatus.Cancelled);
            case null:
                Console.WriteLine("Gate Answer null");
                return new ActionExecutionResult(ActionExecutionStatus.Error);
        }

        Console.WriteLine("GATE GOT MESSAGE " + msg.Text);
        var option = Options.FirstOrDefault(o => o.IsCorrect(msg.Text));
        //  Если юзер норм ответил 
        if (option is not null)
        {
            var action = option.Action;
            Console.WriteLine(action.GetType());
            // Допустимы:
            //  Отправка сообщений
            //  Переходы на другие экраны
            if (action.GetType() == typeof(TextMessage))
            {
                await action.Run(ctx);
                return new ActionExecutionResult(ActionExecutionStatus.RunNext);    
            }

            if (action.GetType() == typeof(Redirect))
            {
                return new ActionExecutionResult(ActionExecutionStatus.SwitchWindow, action as Redirect);    
            }
        }
        // Обработка если ничего не совплао 
        if (DefaultAction is null)
        {
            _botClient.SendTextMessageAsync(_chatId,
                "Нормальное разговаривай: ваш ответ не соотвествует ни одному варианту");
            return new ActionExecutionResult(ActionExecutionStatus.ReRun);
        }
        await DefaultAction.Run(ctx);
        return new ActionExecutionResult(ActionExecutionStatus.RunNext);
    }
}