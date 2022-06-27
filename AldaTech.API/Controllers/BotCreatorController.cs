namespace AldaTech_api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AldaTech_api.Models;
using System.IO;

[Route("/api/[controller]")]
[ApiController]
public class BotCreatorController : Controller
{
    ApplicationContext _dbCtx;
    public BotCreatorController(ApplicationContext dbCtx)
    {
        _dbCtx = dbCtx;
    }
    [HttpGet]
    public async Task<ActionResult> GetBots()
    {
        var bots = await _dbCtx.Bots.ToListAsync();
        return Ok(bots);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult> GetBots(int id)
    {
        var bot = _dbCtx.Bots.FirstOrDefault(bot => bot.Id == id);
        if (bot is null)
            return NotFound();
        return Ok(bot);
    }
    
    [HttpPost]
    public async Task<ActionResult> PostBot(string token, string title)
    {
        Console.WriteLine(1);
        var bot = new Bot(){Token = token, Title = title};
        Console.WriteLine(2);
        if (!ModelState.IsValid) {
            Console.WriteLine("1");
            return BadRequest(ModelState);
        }
        Console.WriteLine(3);
        bot.BotManagerPath = $"./Data/{DateTime.Now.ToFileTime()}.json";
        Console.WriteLine(4);
        await using (var fs = System.IO.File.Create(bot.BotManagerPath)) { }
        
        
        DirectoryInfo d = new DirectoryInfo(@"./Data"); //Assuming Test is your Folder
        FileInfo[] Files = d.GetFiles(); //Getting Text files
        string str = "";
        foreach(FileInfo file in Files )
        {
            Console.WriteLine(file.Name);
        }
        
        
        bot.IsRunning = false;
        var newBot = await _dbCtx.AddAsync(bot);
        Console.WriteLine(6);
        if (newBot == null) {
            Console.WriteLine("2");
            return BadRequest("Unable to insert customer");
        }
        Console.WriteLine(7);
        Console.WriteLine("3");
        await _dbCtx.SaveChangesAsync();
        return Ok(bot);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBot(int id)
    {
        var bot = _dbCtx.Bots.FirstOrDefault(bot => bot.Id == id);
        if (bot is null)
            return NotFound();
        if (bot.IsRunning)
            return Ok("Bot is running. Stop bot before deleting");
        _dbCtx.Bots.Remove(bot);
        if(System.IO.File.Exists(bot.BotManagerPath))
        {
            System.IO.File.Delete(bot.BotManagerPath);
        }

        await _dbCtx.SaveChangesAsync();
        return Ok("Bot deleted");
    }
    
}