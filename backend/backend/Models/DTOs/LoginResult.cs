namespace backend.Models.DTOs;

// se envia al iniciar sesion
public class LoginResult
{
    public int Id { get; set; }

    public string AccessToken { get; set; }

    public string Name { get; set; }
}
