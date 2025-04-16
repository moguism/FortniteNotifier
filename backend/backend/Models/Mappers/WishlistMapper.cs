using backend.Models.DTOs;

namespace backend.Models.Mappers;

public class WishlistMapper
{
    public WishlistDto ToDto(Wishlist wishlist)
    {
        return new WishlistDto
        {
            Id = wishlist.Id,
            Item = wishlist.Item,
        };
    }
    public IEnumerable<WishlistDto> ToDto(IEnumerable<Wishlist> wishlists)
    {
        return wishlists.Select(ToDto);
    }
}
