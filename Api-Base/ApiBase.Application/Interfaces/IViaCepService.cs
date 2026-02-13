using System.Threading.Tasks;

namespace ApiBase.Infrastructure.Services;

public interface IViaCepService
{
    Task<bool> CepExistsAsync(string cep);
}

