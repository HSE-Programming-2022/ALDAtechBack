namespace AldaTech_api.Models;

public class Bot
{
	public int Id { get; set; }
	public string Title {get; set; }
	public string Token {get; set; }
	public string BotManagerPath { get; set; }
	public bool IsRunning { get; set; }
}