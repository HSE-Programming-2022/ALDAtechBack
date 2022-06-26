using System.Xml;
using AldaTech_api.BotCore;
using AldaTech_api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AldaTech_api.Controllers;

public class BotResponse
{
    public int BotId { get; set; }
    public string BotCode { get; set; }
}

[Route("/api/[controller]")]
[ApiController]
public class BotCodeController : Controller
{
    ApplicationContext _dbCtx;

    public BotCodeController(ApplicationContext dbCtx)
    {
        _dbCtx = dbCtx;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult> GetScript(int id)
    {
        // var bot = _dbCtx.Bots.FirstOrDefault(bot => bot.Id == id);
        // if (bot is null)
        //     return NotFound();
        string json;
        
        
        using (StreamReader r = new StreamReader("./Data/1.json"))
        {
            json = r.ReadToEnd();
        }
        
        
        BotManager botCode = BotJsonStorage.ReadBotManager("./Data/1.json");
        return Content(json, "application/json");
    }
    
    [HttpPost("{id}")]
    public async Task<ActionResult> WriteScript(BotResponse br)
    {
        Console.WriteLine(br.BotCode);
        Console.WriteLine(br.BotId);

        var botInfo = _dbCtx.Bots.FirstOrDefault(bot => bot.Id == br.BotId);
        if (botInfo is null)
            return NotFound();
        
        var bot = BotJsonStorage.ReadBotManagerFromString(br.BotCode);
        BotJsonStorage.SaveBotManager(botInfo.BotManagerPath, bot);
        
        return Ok();
    }
    
    
}