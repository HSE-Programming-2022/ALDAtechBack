namespace AldaTech_api.BotFactory;

public class BotComponentData
{
    // Общая штука
    public string Type { get; set; }

    // Дети 
    public List<BotComponentData>? Children { get; set; }
    
    // TextMessage
    public string? Text { get; set; }

    // Клавиатура
    public string? ButtonText { get; set; }
    
    // Переход
    public int? RedirectScreenId { get; set; }

    // Развилка - только дети 
    
    // Ответы option
    public string? GateOptionText { get; set; }

}