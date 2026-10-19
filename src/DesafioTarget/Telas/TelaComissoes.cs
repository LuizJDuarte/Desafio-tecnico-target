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

        Console.WriteLine();
        Console.WriteLine("----- Comissão dos vendedores -----");
        Console.WriteLine("Regra: abaixo de R$ 100,00 não gera comissão, abaixo de R$ 500,00 gera 1%, a partir de R$ 500,00 gera 5%");
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
