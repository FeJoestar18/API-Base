using System.ComponentModel.DataAnnotations;

namespace ApiBase.Application.DTOs;

public class AddressDto
{
    [Required]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "CEP deve conter 8 dígitos numéricos.")]
    public string Cep { get; set; } = null!;

    [Required]
    public string Street { get; set; } = null!;

    [Required]
    public string Number { get; set; } = null!;

    public string? Complement { get; set; }

    [Required]
    public string Neighborhood { get; set; } = null!;

    [Required]
    public string City { get; set; } = null!;

    [Required]
    public string State { get; set; } = null!;
}

public class RegisterDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = null!;

    [Required]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF deve conter exatamente 11 dígitos.")]
    public string Cpf { get; set; } = null!;

    [Required]
    [StringLength(45, ErrorMessage = "Nome deve ter no máximo 45 caracteres.")]
    public string Name { get; set; } = null!;

    [Required]
    public AddressDto Address { get; set; } = null!;
}
