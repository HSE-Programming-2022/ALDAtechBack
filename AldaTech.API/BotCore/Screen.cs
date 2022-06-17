namespace AldaTech_api.BotCore;

public class Screen
{
    public long Id { get; set; }
    private IBotClient _botClient;
    private long _chatId;
    private string _text;
    List<IBotAction> _actions;

    public Screen(IBotClient botClient, long chatId, List<IBotAction> actions, long id)
    {
        _botClient = botClient;
        _chatId = chatId;
        _actions = actions;
        Id = id;
    }

    public async Task<Redirect?> Run()
    {
        foreach (var action in _actions)
        {
            var execRes = await action.Run();
            if (execRes.Status == ActionExecutionStatus.Error)
            {
                Console.WriteLine("Error");
                return null;
            }

            if (execRes.Status == ActionExecutionStatus.SwitchWindow)
            {
                return execRes.Redirect;
            }
            
            // Можно ничего не писать 
            if (execRes.Status == ActionExecutionStatus.RunNext)
                continue;
        }

        return null;
    }
}