using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiBase.Infrastructure.Services;

public class ViaCepService : IViaCepService
{
    private readonly HttpClient _http;

    public ViaCepService(HttpClient http)
    {
        _http = http;
    }

    public async Task<bool> CepExistsAsync(string cep)
    {
        if (string.IsNullOrWhiteSpace(cep)) return false;
        cep = new string(cep.Where(char.IsDigit).ToArray());
        if (cep.Length != 8) return false;

        var res = await _http.GetAsync($"/ws/{cep}/json/");
        if (!res.IsSuccessStatusCode) return false;

        using var stream = await res.Content.ReadAsStreamAsync();
        try
        {
            var doc = await JsonDocument.ParseAsync(stream);

            if (doc.RootElement.TryGetProperty("erro", out var erro))
            {
                if (erro.ValueKind == JsonValueKind.True) return false;
            }

            if (doc.RootElement.TryGetProperty("cep", out var cepProp) && cepProp.ValueKind == JsonValueKind.String && !string.IsNullOrWhiteSpace(cepProp.GetString()))
            {
                
                var hasUf = doc.RootElement.TryGetProperty("uf", out var ufProp) && ufProp.ValueKind == JsonValueKind.String && !string.IsNullOrWhiteSpace(ufProp.GetString());
                var hasLocalidade = doc.RootElement.TryGetProperty("localidade", out var locProp) && locProp.ValueKind == JsonValueKind.String && !string.IsNullOrWhiteSpace(locProp.GetString());

                if (hasUf || hasLocalidade)
                    return true;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }
}
