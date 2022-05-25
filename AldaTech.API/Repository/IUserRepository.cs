using System.Collections.Generic;
using System.Threading.Tasks;
namespace AldaTech_api.Models;

public interface IUserRepository
{
        Task<List<User>> GetUsersAsync();

        Task<User> GetUserAsync(int id);
        
        Task<User> InsertUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
}
