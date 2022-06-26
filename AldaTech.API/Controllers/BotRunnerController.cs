using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AldaTech_api.Models;

using AldaTech_api.BotCore;

namespace AldaTech_api.Controllers;


[Route("/api/[controller]")]
public class BotRunnerController : Controller
{
	private BotRunner _botRunner;
	public BotRunnerController(BotRunner botRunner)
	{
		_botRunner = botRunner;
	}
	
	[HttpPost]
	[Route("StartBot")]
	// public async Task<ActionResult> StartBot(int id)
	// {
	// 	return Ok(await _botRunner.StartBot(id));
	// }
	public async Task<ActionResult> StartBot(int id)
	{
		var bc = new BotManager();
		return Ok();
	}

	
	[HttpPost]
	[Route("StopBot")]
	public async Task<ActionResult> StopBot(int id)
	{
		return Ok(await _botRunner.StopBot(id));
	}
	
	[HttpPost]
	[Route("RestartBot")]
	public async Task<ActionResult> RestartBot(int id)
	{
		return Ok(await _botRunner.RestartBot(id));
	}
}