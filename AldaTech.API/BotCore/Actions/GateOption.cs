using AldaTech_api.BotFactory;

namespace AldaTech_api.BotCore;

public class GateOption
{
    public string TextOption {get; set; }
 
    public IBotAction Action {get; set; }

    public GateOption(string textOption, IBotAction action)
    {
        TextOption = textOption;
        Action = action;
    }
    
    public GateOption(BotComponentData botComponentData)
    {
        TextOption = botComponentData.GateOptionText;
        var actionData = botComponentData.Children[0];

        if (actionData.Type == "Redirect")
        {
            Action = new Redirect(actionData);
        }
        else if (actionData.Type == "TextMessage")
        {
            Action = new TextMessage(actionData);
        }
    }

    public bool IsCorrect(string q) => string.Equals(q, TextOption, StringComparison.CurrentCultureIgnoreCase);

}