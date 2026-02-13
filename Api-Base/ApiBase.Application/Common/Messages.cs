namespace ApiBase.Application.Common;

public static class Messages
{
    // Auth / User
    public const string InvalidCpf = "CPF inválido: deve conter exatamente 11 dígitos numéricos.";
    public const string InvalidName = "Nome inválido: máximo 45 caracteres.";
    public const string CpfAlreadyRegistered = "CPF já cadastrado.";
    public const string EmailAlreadyRegistered = "Email já cadastrado.";
    public const string UserNotFound = "Usuário não encontrado";
    public const string InvalidPassword = "Senha inválida";

    // Address / CEP
    public const string InvalidCep = "CEP inválido ou não encontrado.";

    // Generic
    public const string UnexpectedError = "Ocorreu um erro inesperado.";
}
