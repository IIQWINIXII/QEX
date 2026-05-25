namespace QEX_Server.Dtos
{
    public record RegisterRequest(string UserName, string Password);
    public record LoginRequest(string UserName, string Password);
    public record AuthResponse(bool Success, string Message, string? Token);
}
