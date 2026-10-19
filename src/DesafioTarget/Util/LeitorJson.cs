using System.Text.Json;

namespace DesafioTarget.Util;

public static class LeitorJson
{
    // Os JSONs do desafio usam camelCase ("vendedor", "valor"), as classes usam PascalCase
    private static readonly JsonSerializerOptions Opcoes = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static T Ler<T>(string nomeArquivo)
    {
        // A pasta Dados é copiada pro bin no build (ver o .csproj)
        var caminho = Path.Combine(AppContext.BaseDirectory, "Dados", nomeArquivo);

        if (!File.Exists(caminho))
        {
            throw new FileNotFoundException($"Arquivo {nomeArquivo} não encontrado em {caminho}");
        }

        var json = File.ReadAllText(caminho);

        return JsonSerializer.Deserialize<T>(json, Opcoes)
            ?? throw new InvalidOperationException($"O arquivo {nomeArquivo} está vazio.");
    }
}
