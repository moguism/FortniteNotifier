namespace backend.Models.DTOs;

// datos con los que el usuario inicia sesion
public class LoginRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;

}
