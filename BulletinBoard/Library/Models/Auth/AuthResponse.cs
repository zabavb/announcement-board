using Library.Models.Dto;

namespace Library.Models.Auth;

public class AuthResponse
{
    public string Token { get; set; }
    public int ExpiresInDays { get; set; }
    public UserDto User { get; set; }

    public AuthResponse(string token, int expiresInDays, UserDto user)
    {
        Token = token;
        ExpiresInDays = expiresInDays;
        User = user;
    }
}