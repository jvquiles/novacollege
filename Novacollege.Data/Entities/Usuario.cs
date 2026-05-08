namespace Novacollege.Data.Entities;

public class Usuario
{
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public DateTimeOffset FechaCreacion { get; set; } = DateTimeOffset.UtcNow;
}