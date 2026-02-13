namespace ApiBase.Domain.Entities;

using System.Collections.Generic;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;

    // Novo: CPF e Nome
    public string Cpf { get; set; } = null!;
    public string Name { get; set; } = null!;

    // Relação para endereços do usuário
    public ICollection<UserAddress> Addresses { get; set; } = new List<UserAddress>();
}