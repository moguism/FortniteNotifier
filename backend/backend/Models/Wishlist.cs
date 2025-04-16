namespace backend.Models;
public class Wishlist
{
    public int Id { get; set; }
    public User User { get; set; }
    public int UserId { get; set; }
    public string Item { get; set; }
}
