using Newtonsoft.Json;

namespace AldaTech_api.BotCore;

public class Screen
{
    [JsonProperty]
    public long Id  { get; set; }
    
    [JsonProperty]
    public List<IBotAction> Actions  { get; set; }
    
    
    private IBotClient _botClient;
    private long _chatId;
    
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