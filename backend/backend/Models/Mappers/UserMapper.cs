using backend.Models.DTOs;

namespace backend.Models.Mappers;

public class UserMapper
{
    private static WishlistMapper _wishlistMapper;

    public UserMapper()
    {
        _wishlistMapper ??= new WishlistMapper();
    }

    public UserDto ToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            Wishlists = [.. _wishlistMapper.ToDto(user.Wishlists)]
        };
    }

    public IEnumerable<UserDto> ToDto(IEnumerable<User> users)
    {
        return users.Select(ToDto);
    }
}
