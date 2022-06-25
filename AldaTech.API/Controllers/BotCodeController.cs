using System.Xml;
using AldaTech_api.BotCore;
using AldaTech_api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AldaTech_api.Controllers;

public class Response
{
    public int BotId { get; set; }
    public BotManager BotManager { get; set; }
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
    
}