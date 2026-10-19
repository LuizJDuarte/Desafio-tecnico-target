namespace DesafioTarget.Comissoes;

public class ComissaoService
{
    private const decimal ValorMinimoParaComissao = 100m;
    private const decimal ValorMinimoParaCincoPorCento = 500m;
    private const decimal UmPorCento = 0.01m;
    private const decimal CincoPorCento = 0.05m;

    public decimal CalcularComissao(decimal valorVenda)
    {
        // Abaixo de R$ 100,00 não gera comissão
        if (valorVenda < ValorMinimoParaComissao)
        {
            return 0m;
        }

        // De R$ 100,00 até abaixo de R$ 500,00 gera 1%
        if (valorVenda < ValorMinimoParaCincoPorCento)
        {
            return valorVenda * UmPorCento;
        }

        // A partir de R$ 500,00 gera 5%
        return valorVenda * CincoPorCento;
    }

    public List<ComissaoVendedor> CalcularComissoes(List<Venda> vendas)
    {
        return vendas
            .GroupBy(venda => venda.Vendedor)
            .Select(grupo => new ComissaoVendedor(
                grupo.Key,
                grupo.Count(),
                grupo.Sum(venda => venda.Valor),
                // Arredonda só o total do vendedor, arredondar venda por venda acumula diferença de centavos
                Math.Round(grupo.Sum(venda => CalcularComissao(venda.Valor)), 2, MidpointRounding.AwayFromZero)))
            .ToList();
    }
}
