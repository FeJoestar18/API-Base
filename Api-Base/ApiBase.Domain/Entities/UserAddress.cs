namespace ApiBase.Domain.Entities;

public class UserAddress
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    public string Cep { get; set; } = null!; // 8 dígitos
    public string Street { get; set; } = null!;
    public string Number { get; set; } = null!;
    public string? Complement { get; set; }
    public string Neighborhood { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;

    // Navigation
    public User User { get; set; } = null!;
}
