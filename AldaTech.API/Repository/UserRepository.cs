using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace AldaTech_api.Models;

public class UserRepository : IUserRepository
{
	private readonly ApplicationContext _context;

	public UserRepository(ApplicationContext context) {
          _context = context;
        }

	public async Task<List<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserAsync(int id)
        {
            return await _context.Users.SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<User> InsertUserAsync(User User)
        {
            _context.Add(User);
            try
            {
              await _context.SaveChangesAsync();
            }
            catch (System.Exception exp)
            {
				Console.WriteLine($"Error in {nameof(InsertUserAsync)}: " + exp.Message);
            }

            return User;
        }

        public async Task<bool> UpdateUserAsync(User User)
        {
            //Will update all properties of the User
            _context.Users.Attach(User);
            _context.Entry(User).State = EntityState.Modified;
            try
            {
              return (await _context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (Exception exp)
            {
               Console.WriteLine($"Error in {nameof(UpdateUserAsync)}: " + exp.Message);
            }
            return false;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            //Extra hop to the database but keeps it nice and simple for this demo
            var User = await _context.Users.SingleOrDefaultAsync(c => c.Id == id);
            // _context.Remove(x => x.Id = id);
            try
            {
              return (await _context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (System.Exception exp)
            {
               Console.WriteLine($"Error in {nameof(DeleteUserAsync)}: " + exp.Message);
            }
            return false;
        }

}