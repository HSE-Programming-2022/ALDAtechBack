using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AldaTech_api.Models;

namespace AldaTech_api.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController : Controller
{
// 	UserRepository _repo;
// 	public UserController(UserRepository repo)
// 	{
// 		_repo = repo;
// 	}
	[HttpGet]
	public string GetUsers(){
		return "ok";
		// var res = new List<User>();
		// res.Add(new User(){
		// 	Id = 5,
		// 	Name = "Hui",
		// });
		// return res;
	}
	// public async Task<ActionResult> GetUsers()
	// {
	// 	var users = await _repo.GetUsersAsync();
	// 	if (users == null) {
	// 		Console.WriteLine("---");
	// 		return NotFound();
	// 	}
	// 	return Ok(users);
	// }


	// [HttpPost]
	// public async Task<ActionResult> PostCustomer(User user)
    //     {
    //       if (!ModelState.IsValid) {
	// 		  Console.WriteLine("1");
    //         return BadRequest(this.ModelState);
    //       }
    //       var newUser = await _repo.InsertUserAsync(user);
    //       if (newUser == null) {
	// 		  Console.WriteLine("2");
    //         return BadRequest("Unable to insert customer");
    //       }
	// 	  Console.WriteLine("3");
    //       return CreatedAtRoute("GetCustomersRoute", new { id = newUser.Id}, newUser);
    //     }
}