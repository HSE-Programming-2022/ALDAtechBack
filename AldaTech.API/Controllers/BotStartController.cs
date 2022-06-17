using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AldaTech_api.Models;

using AldaTech_api.BotCore;

namespace AldaTech_api.Controllers;


[Route("[controller]")]
[ApiController]
public class BotStartController : Controller
{
	[HttpPost]
	public async Task<ActionResult> StartBot()
	{
		// var TgBotRunner = provider.GetService<IMessageSender>();
		var bot = new BotClientTelegram("5001678276:AAH0MgT6aPNR7pkOGeD5eYFxEsJI3sgE4WA");
		Console.WriteLine("Bot started");
		return Ok();
	}

}