namespace AldaTech_api.BotCore;

public class Redirect : IBotAction
{
    public long ScreenId { get; set; }

    public Redirect(long screenId)
    {
        ScreenId = screenId;
    }
    public async Task<ActionExecutionResult> Run()
    {
        Console.WriteLine("Running Redirect");
        return new ActionExecutionResult(ActionExecutionStatus.SwitchWindow, this);
    }
    
}