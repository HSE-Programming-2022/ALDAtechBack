using AldaTech_api.BotFactory;
using Newtonsoft.Json;

namespace AldaTech_api.BotCore;

public class Screen
{
    public long Id  { get; set; }
    public List<IBotAction> Actions  { get; set; }
    
    
    private IBotClient _botClient;
    private long _chatId;
    
    public Screen(ScreenData screenData)
    {
        Id = screenData.Id;
        Actions = new List<IBotAction>();
        foreach (var component in screenData.Components)
        {
            IBotAction botAction = null;
            if (component.Type == "TextMessage")
            {
                botAction = new TextMessage(component);
            }
            else if (component.Type == "Gate")
            {
                botAction = new Gate(component);
            }
            else if (component.Type == "KeyBoard")
            {
                botAction = new KeyBoard(component);
            }
            else if (component.Type == "Redirect")
            {
                botAction = new Redirect(component);
            }
            Actions.Add(botAction);
        }
    }
    
    public Screen(IBotClient botClient, long chatId, List<IBotAction> actions, long id)
    {
        _botClient = botClient;
        _chatId = chatId;
        Actions = actions;
        Id = id;
    }

    public async Task<ActionExecutionResult> Run(BotUserContext ctx)
    {
        _botClient = ctx.BotClient;
        _chatId = ctx.ChatId; 
        foreach (var action in Actions)
        {
            Console.WriteLine(ctx.Ct.IsCancellationRequested);
            if (ctx.Ct.IsCancellationRequested)
                return new ActionExecutionResult(ActionExecutionStatus.Cancelled);

            var executionResult = await action.Run(ctx);
            while (executionResult.Status == ActionExecutionStatus.ReRun)
            {
                executionResult = await action.Run(ctx);
            }
            Console.WriteLine("Screen " + executionResult.Status);
            switch (executionResult.Status)
            {
                case ActionExecutionStatus.Cancelled:
                    return executionResult;
                    break;
                case ActionExecutionStatus.Error:
                    return executionResult;
                    break;
                case ActionExecutionStatus.RunNext:
                    continue;
                    break;
                case ActionExecutionStatus.SwitchWindow:
                    return executionResult;
                    break;
            }
        }
        return new ActionExecutionResult(ActionExecutionStatus.Done);
    }
}