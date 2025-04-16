using backend.Models;
using backend.Models.DTOs;

namespace backend.Services;

public class WishlistService
{
    private readonly UnitOfWork _unitOfWork;

    public WishlistService(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Wishlist> CreateWishlistAsync(WishlistDto wishlistDto, int userId)
    {
        Wishlist alreadyExistingWishlist = await _unitOfWork.WishlistRepository.SearchWishlistByUserAsync(userId, wishlistDto.Item);
        if(alreadyExistingWishlist != null)
        {
            Console.WriteLine("Ya existe una wishlist para este usuario con este item");
            return null;
        }

        Wishlist wishlist = await _unitOfWork.WishlistRepository.InsertAsync(new Wishlist()
        {
            Item = wishlistDto.Item,
            UserId = userId
        });

        await _unitOfWork.SaveAsync();
        
        return wishlist;
    }

    public async Task DeleteWishlistOfUserByIdAsync(int wishlistId, User user)
    {
        Wishlist wishlist = await _unitOfWork.WishlistRepository.GetByIdAsync(wishlistId);
        if(wishlist == null || wishlist.UserId != user.Id)
        {
            return;
        }

        _unitOfWork.WishlistRepository.Delete(wishlist);
        await _unitOfWork.SaveAsync();
    }
}
