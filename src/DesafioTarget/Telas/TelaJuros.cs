using DesafioTarget.Juros;
using DesafioTarget.Util;

namespace DesafioTarget.Telas;

public class TelaJuros
{
    private readonly JurosService _jurosService;

    public TelaJuros(JurosService jurosService)
    {
        _jurosService = jurosService;
    }

    public void Exibir()
    {
        Console.WriteLine();
        Console.WriteLine("----- Cálculo de juros por atraso -----");

        var valor = ConsoleHelper.LerValor("Valor (ex: 1500,00): ");
        var vencimento = ConsoleHelper.LerData("Data de vencimento (dd/mm/aaaa): ");
        var hoje = DateOnly.FromDateTime(DateTime.Today);

        try
        {
            var calculo = _jurosService.Calcular(valor, vencimento, hoje);

            Console.WriteLine();
            Console.WriteLine($"Valor original:      {ConsoleHelper.FormatarMoeda(calculo.ValorOriginal)}");
            Console.WriteLine($"Vencimento:          {ConsoleHelper.FormatarData(calculo.DataVencimento)}");
            Console.WriteLine($"Data do cálculo:     {ConsoleHelper.FormatarData(calculo.DataCalculo)}");
            Console.WriteLine($"Dias em atraso:      {calculo.DiasAtraso}");
            Console.WriteLine($"Taxa:                {ConsoleHelper.FormatarPercentual(JurosService.TaxaDiaria)} ao dia");
            Console.WriteLine($"Juros:               {ConsoleHelper.FormatarMoeda(calculo.ValorJuros)}");
            Console.WriteLine($"Valor atualizado:    {ConsoleHelper.FormatarMoeda(calculo.ValorAtualizado)}");

            if (calculo.DiasAtraso == 0)
            {
                Console.WriteLine("Ainda não passou do vencimento, então não tem juros.");
            }
        }
        catch (ArgumentException ex)
        {
            ConsoleHelper.Erro(ex.Message);
        }
    }
}
