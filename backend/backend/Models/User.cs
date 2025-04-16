namespace backend.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public List<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
