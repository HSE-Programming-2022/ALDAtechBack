using Telegram.Bot.Types;

namespace AldaTech_api.BotCore;

public class MessageListener
{
    public TaskCompletionSource<Message?> Task { get; set; }
    private Func<Message, bool> Check;

    public MessageListener(Func<Message, bool> check)
    {
        this.Check = check;
        this.Task = new TaskCompletionSource<Message>();
    }

    public bool ListensTo(Message message) => Check(message);
}