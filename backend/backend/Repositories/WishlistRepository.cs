using backend.Models;
using backend.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class WishlistRepository : Repository<Wishlist, int>
{
    public WishlistRepository(Context context) : base(context) { }

    public async Task<Wishlist> SearchWishlistByUserAsync(int userId, string item)
    {
        return await GetQueryable().FirstOrDefaultAsync(w => w.UserId == userId && w.Item.Equals(item));
    }
}