using DesafioTarget.Comissoes;
using DesafioTarget.Util;

namespace DesafioTarget.Telas;

public class TelaComissoes
{
    private readonly ComissaoService _comissaoService;
    private readonly List<Venda> _vendas;

    public TelaComissoes(ComissaoService comissaoService, List<Venda> vendas)
    {
        _comissaoService = comissaoService;
        _vendas = vendas;
    }

    public void Exibir()
    {
        var comissoes = _comissaoService.CalcularComissoes(_vendas);

        // A regra mostrada vem das constantes do service, então tela e cálculo nunca ficam diferentes
        var minimo = ConsoleHelper.FormatarMoeda(ComissaoService.ValorMinimoParaComissao);
        var faixaMaior = ConsoleHelper.FormatarMoeda(ComissaoService.ValorMinimoFaixaMaior);
        var percentualMenor = ConsoleHelper.FormatarPercentual(ComissaoService.PercentualFaixaMenor);
        var percentualMaior = ConsoleHelper.FormatarPercentual(ComissaoService.PercentualFaixaMaior);

        Console.WriteLine();
        Console.WriteLine("----- Comissão dos vendedores -----");
        Console.WriteLine($"Regra: abaixo de {minimo} não gera comissão, abaixo de {faixaMaior} gera {percentualMenor}, a partir de {faixaMaior} gera {percentualMaior}");
        Console.WriteLine();
        Console.WriteLine($"{"Vendedor",-18}{"Vendas",7}{"Total vendido",17}{"Comissão",14}");

        foreach (var comissao in comissoes)
        {
            var totalVendido = ConsoleHelper.FormatarMoeda(comissao.TotalVendido);
            var totalComissao = ConsoleHelper.FormatarMoeda(comissao.TotalComissao);

            Console.WriteLine($"{comissao.Vendedor,-18}{comissao.QuantidadeVendas,7}{totalVendido,17}{totalComissao,14}");
        }

        Console.WriteLine();
        Console.WriteLine($"Total de comissões: {ConsoleHelper.FormatarMoeda(comissoes.Sum(c => c.TotalComissao))}");
    }
}
