using AldaTech_api.BotFactory;
using Newtonsoft.Json;

namespace AldaTech_api.BotCore;

public class Redirect : IBotAction
{
    public int ScreenId { get; set; }

    public Redirect(BotComponentData redirectData)
    {
        if (redirectData.RedirectScreenId is not null)
        {
            ScreenId = (int) redirectData.RedirectScreenId;
        }
    }
    public Redirect(int screenId)
    {
        ScreenId = screenId;
    }
    public async Task<ActionExecutionResult> Run(BotUserContext ctx)
    {
        Console.WriteLine("Running Redirect");
        return new ActionExecutionResult(ActionExecutionStatus.SwitchWindow, this);
    }
    
}