namespace backend.Models.DTOs;

// datos con los que el usuario se registra
public class RegisterDto
{
    public int Id { get; set; } = 0;
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
}
