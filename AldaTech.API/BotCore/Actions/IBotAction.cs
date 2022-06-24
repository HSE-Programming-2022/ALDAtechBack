namespace AldaTech_api.BotCore;

public interface IBotAction
{
    public Task<ActionExecutionResult> Run(BotUserContext ctx);
}