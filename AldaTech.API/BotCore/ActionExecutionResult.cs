namespace AldaTech_api.BotCore;

public class ActionExecutionResult
{
    public ActionExecutionResult(ActionExecutionStatus status)
    {
        Status = status;
    }

    public ActionExecutionResult(ActionExecutionStatus status, Redirect redirect)
    {
        Status = status;
        Redirect = redirect;
    }
    
    public ActionExecutionStatus Status { get; set; }
    public Redirect Redirect { get; }
    
}