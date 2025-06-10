namespace Core.Service;

public static class PasswordHasher
{
    private static readonly int WorkFactor = 12;

    public static string HashPassword(string Password)
    {
        return BCrypt.Net.BCrypt.HashPassword(Password, WorkFactor);
    }

    public static bool VerifyPassword(string hashedPassword, string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}