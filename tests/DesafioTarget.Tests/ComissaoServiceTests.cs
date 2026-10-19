using DesafioTarget.Comissoes;
using DesafioTarget.Util;

namespace DesafioTarget.Tests;

public class ComissaoServiceTests
{
    private readonly ComissaoService _service = new();

    [Fact]
    public void VendaAbaixoDe100_NaoGeraComissao()
    {
        Assert.Equal(0m, _service.CalcularComissao(99.99m));
        Assert.Equal(0m, _service.CalcularComissao(90.75m));
    }

    [Fact]
    public void VendaDe100AteAbaixoDe500_GeraUmPorCento()
    {
        Assert.Equal(1.00m, _service.CalcularComissao(100m));
        Assert.Equal(4.9999m, _service.CalcularComissao(499.99m));
    }

    [Fact]
    public void VendaAPartirDe500_GeraCincoPorCento()
    {
        Assert.Equal(25.00m, _service.CalcularComissao(500m));
        Assert.Equal(60.025m, _service.CalcularComissao(1200.50m));
    }

    [Fact]
    public void CalcularComissoes_DeveSomarPorVendedorEArredondarSoNoTotal()
    {
        var vendas = new List<Venda>
        {
            new() { Vendedor = "João Silva", Valor = 1200.50m }, // 60,025
            new() { Vendedor = "João Silva", Valor = 950.75m },  // 47,5375
            new() { Vendedor = "Maria Souza", Valor = 90.75m }   // sem comissão
        };

        var comissoes = _service.CalcularComissoes(vendas);

        var joao = comissoes.Single(c => c.Vendedor == "João Silva");
        Assert.Equal(2, joao.QuantidadeVendas);
        Assert.Equal(2151.25m, joao.TotalVendido);
        Assert.Equal(107.56m, joao.TotalComissao); // 107,5625 (arredondando venda por venda daria 107,57)

        var maria = comissoes.Single(c => c.Vendedor == "Maria Souza");
        Assert.Equal(0m, maria.TotalComissao);
    }

    [Fact]
    public void CalcularComissoes_ComOsDadosDoDesafio()
    {
        var vendas = LeitorJson.Ler<ArquivoVendas>("vendas.json").Vendas;

        var comissoes = _service.CalcularComissoes(vendas);

        Assert.Equal(4, comissoes.Count);
        Assert.Equal(495.68m, comissoes.Single(c => c.Vendedor == "João Silva").TotalComissao);
        Assert.Equal(465.95m, comissoes.Single(c => c.Vendedor == "Maria Souza").TotalComissao);
        Assert.Equal(379.37m, comissoes.Single(c => c.Vendedor == "Carlos Oliveira").TotalComissao);
        Assert.Equal(404.98m, comissoes.Single(c => c.Vendedor == "Ana Lima").TotalComissao);
    }
}
