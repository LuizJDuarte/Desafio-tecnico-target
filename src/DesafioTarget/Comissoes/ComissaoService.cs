namespace DesafioTarget.Comissoes;

public class ComissaoService
{
    // Públicas porque a tela de comissões usa os mesmos valores pra mostrar a regra
    public const decimal ValorMinimoParaComissao = 100m;
    public const decimal ValorMinimoFaixaMaior = 500m;
    public const decimal PercentualFaixaMenor = 0.01m;
    public const decimal PercentualFaixaMaior = 0.05m;

    public decimal CalcularComissao(decimal valorVenda)
    {
        // Abaixo de R$ 100,00 não gera comissão
        if (valorVenda < ValorMinimoParaComissao)
        {
            return 0m;
        }

        // De R$ 100,00 até abaixo de R$ 500,00 gera 1%
        if (valorVenda < ValorMinimoFaixaMaior)
        {
            return valorVenda * PercentualFaixaMenor;
        }

        // A partir de R$ 500,00 gera 5%
        return valorVenda * PercentualFaixaMaior;
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
