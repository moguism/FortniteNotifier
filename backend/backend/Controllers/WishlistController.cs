using backend.Models;
using backend.Models.DTOs;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WishlistController : ControllerBase
{
    private readonly UserService _userService;
    private readonly WishlistService _wishlistService;
    private readonly PlaywrightService _playwrightService;

    public WishlistController(UserService userService, WishlistService wishlistService, PlaywrightService playwrightService)
    {
        _userService = userService;
        _wishlistService = wishlistService;
        _playwrightService = playwrightService;
    }

    [Authorize]
    [HttpPost]
    public async Task<bool> CreateWishlist([FromBody] WishlistDto wishlistDto)
    {
        User user = await GetAuthorizedUser();

        if (user == null)
        {
            Console.WriteLine("El usuario es nulo");
            return false;
        }

        await _wishlistService.CreateWishlistAsync(wishlistDto, user.Id);

        List<string> elementsFound = await _playwrightService.DoWebScrapping([wishlistDto.Item]);

        bool found = elementsFound.Count == 1;
        return found;
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task DeleteWishlistById(int id)
    {
        User user = await GetAuthorizedUser();

        if (user == null)
        {
            Console.WriteLine("El usuario es nulo");
            return;
        }

        await _wishlistService.DeleteWishlistOfUserByIdAsync(id, user);
    }

    private async Task<User> GetAuthorizedUser()
    {
        System.Security.Claims.ClaimsPrincipal currentUser = this.User;
        string firstClaim = currentUser.Claims.First().ToString();
        string idString = firstClaim.Substring(firstClaim.IndexOf("nameidentifier:") + "nameIdentifier".Length + 2);

        // Pilla el usuario de la base de datos
        User user = await _userService.GetFullUserByIdAsync(int.Parse(idString));
        return user;
    }
}
