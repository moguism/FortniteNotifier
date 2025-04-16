using Microsoft.EntityFrameworkCore;
using backend.Models;
using System.Text.RegularExpressions;
using backend.Models.Mappers;
using backend.Models.DTOs;
using backend.Models.Helper;

namespace backend.Services;

public class UserService
{
    private readonly UnitOfWork _unitOfWork;
    private readonly UserMapper _userMapper;

    Regex emailRegex = new(@"^[^\s@]+@[^\s@]+\.[^\s@]+$");
    public UserService(UnitOfWork unitOfWork, UserMapper userMapper)
    {
        _unitOfWork = unitOfWork;
        _userMapper = userMapper;
    }

    public async Task<UserDto> GetUserByEmailAsync(string email)
    {
        var user = await _unitOfWork.UserRepository.GetByEmailAsync(email);
        if (user == null)
        {
            return null;
        }
        return _userMapper.ToDto(user);
    }

    public async Task<User> GetFullUserByIdAsync(int id)
    {
        var user = await _unitOfWork.UserRepository.GetFullUserByIdAsync(id);
        return user;
    }

    public async Task<User> GetBasicUserByIdAsync(int id)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
        return user;
    }

    // REGISTRO 
    public async Task<User> RegisterAsync(RegisterDto model)
    {
        model.Email = model.Email?.Trim().ToLower();
        Console.WriteLine("Email recibido: " + model.Email);

        // validacion email

        if (string.IsNullOrEmpty(model.Email) || !emailRegex.IsMatch(model.Email))
        {
            throw new Exception("Email no valido.");
        }

        if (model.Password == null || model.Password.Length < 6)
        {
            throw new Exception("Contraseña no válida");
        }

        try
        {

            // Verifica si el usuario ya existe
            var existingUser = await GetUserByEmailAsync(model.Email.ToLower());

            if (existingUser != null)
            {
                throw new Exception("El usuario ya existe.");
            }

            var newUser = new User
            {
                Name = model.Name,
                Email = model.Email.ToLower(),
                Password = PasswordHelper.Hash(model.Password),
            };

            await _unitOfWork.UserRepository.InsertAsync(newUser);
            await _unitOfWork.SaveAsync();

            return newUser;

        }
        catch (DbUpdateException ex)
        {
            // Log más detallado del error
            Console.WriteLine($"Error al guardar el usuario: {ex.InnerException?.Message}");
            throw new Exception("Error al registrar el usuario. Verifica los datos ingresados.");
        }
    }

    // INICIO DE SESION
    public async Task<User> LoginAsync(LoginRequest loginRequest)
    {
        var user = await _unitOfWork.UserRepository.GetByEmailAsync(loginRequest.Email.ToLower());

        if (user == null || user.Password != PasswordHelper.Hash(loginRequest.Password))
        {
            return null;
        }

        return user;
    }

    public UserDto ToDto(User user)
    {
        UserDto userDto = _userMapper.ToDto(user);
        return userDto;
    }
}
