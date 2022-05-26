namespace AldaTech_api.Models;

public class Screen
{
	public int Id { get; set; }
	public string Title { get; set; }
	// Сохранять позицию на экране, чтобы всегда выглядео одинаково для юзера 

	public List<Component> Components {get; set; }
	
}