using Microsoft.EntityFrameworkCore;
using backend.Models;
using backend.Repositories.Base;

namespace backend.Repositories;

public class UserRepository : Repository<User, int>
{
    public UserRepository(Context context) : base(context) { }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await GetQueryable()
            .Include(u => u.Wishlists)
            .FirstOrDefaultAsync(user => user.Email.Equals(email));
    }

    public async Task<User> GetFullUserByIdAsync(int id)
    {
        return await GetQueryable()
            .Include(u => u.Wishlists)
            .FirstOrDefaultAsync(user => user.Id == id);
    }

    public async Task<List<User>> GetAllFullUsersAsync()
    {
        return await GetQueryable()
            .Include(u => u.Wishlists)
            .ToListAsync();
    }
}