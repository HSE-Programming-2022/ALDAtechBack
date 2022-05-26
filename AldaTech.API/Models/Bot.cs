namespace AldaTech_api.Models;

public class Bot
{
	public int BotID { get; set; }
	public string Title {get; set; }

	public string TgToken {get; set; }

	public List<Screen> Screens {get; set;} // Возможно это лучше сделать в виде словаря 


}