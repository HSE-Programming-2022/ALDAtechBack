using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AldaTech_api.Models;

using AldaTech_api.BotCore;

namespace AldaTech_api.Controllers;


[Route("[controller]")]
[ApiController]
public class BotController : Controller
{
	[HttpPost]
	public async Task<ActionResult> GetUpdates()
	{
		// var TgBotRunner = provider.GetService<IMessageSender>();
		var TgAPI = new TelegramAPI();
		TgAPI.GetUpdates();
		return Ok();
	}

}