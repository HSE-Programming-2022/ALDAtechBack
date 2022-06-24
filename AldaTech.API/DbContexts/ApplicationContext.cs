using Microsoft.EntityFrameworkCore;
 
namespace AldaTech_api.Models;
public class ApplicationContext : DbContext
{
	public DbSet<User> Users { get; set; } = null!;
	public DbSet<Bot> Bots { get; set; } = null!;
	public ApplicationContext(DbContextOptions<ApplicationContext> options)
		: base(options)
	{
		// Database.EnsureDeleted();
		// Database.EnsureCreated();   // создаем базу данных при первом обращении
		// Console.WriteLine("DB initialized");
	}
}
