using System.Globalization;

namespace DesafioTarget.Util;

public static class ConsoleHelper
{
    private static readonly CultureInfo CulturaBr = new("pt-BR");

    public static string LerTexto(string mensagem)
    {
        Console.Write(mensagem);
        var linha = Console.ReadLine();

        // ReadLine devolve null quando a entrada é encerrada (Ctrl+Z / Ctrl+D), aí só fecha o programa
        if (linha == null)
        {
            Environment.Exit(0);
        }

        return linha.Trim();
    }

    public static int LerInteiro(string mensagem)
    {
        while (true)
        {
            var texto = LerTexto(mensagem);

            if (int.TryParse(texto, out var numero))
            {
                return numero;
            }

            Erro("Digite um número inteiro.");
        }
    }

    public static decimal LerValor(string mensagem)
    {
        while (true)
        {
            var texto = LerTexto(mensagem);

            // Só aceita vírgula nos centavos e sem separador de milhar,
            // senão "1500.50" no padrão brasileiro vira 150050 sem ninguém perceber
            if (decimal.TryParse(texto, NumberStyles.AllowDecimalPoint, CulturaBr, out var valor))
            {
                return valor;
            }

            Erro("Valor inválido. Use vírgula nos centavos, ex: 1500,50");
        }
    }

    public static DateOnly LerData(string mensagem)
    {
        while (true)
        {
            var texto = LerTexto(mensagem);

            if (DateOnly.TryParseExact(texto, "dd/MM/yyyy", CulturaBr, DateTimeStyles.None, out var data))
            {
                return data;
            }

            Erro("Data inválida. Use o formato dd/mm/aaaa, ex: 05/10/2026");
        }
    }

    public static string FormatarMoeda(decimal valor)
    {
        return valor.ToString("C", CulturaBr);
    }

    public static string FormatarData(DateOnly data)
    {
        return data.ToString("dd/MM/yyyy", CulturaBr);
    }

    public static string FormatarDataHora(DateTime dataHora)
    {
        return dataHora.ToString("dd/MM/yyyy HH:mm", CulturaBr);
    }

    public static void Erro(string mensagem)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Erro: {mensagem}");
        Console.ResetColor();
    }

    public static void Sucesso(string mensagem)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(mensagem);
        Console.ResetColor();
    }
}
