using DesafioTarget.Estoques;

namespace DesafioTarget.Tests;

public class EstoqueServiceTests
{
    private readonly EstoqueService _service;

    public EstoqueServiceTests()
    {
        // O xUnit cria uma instância nova da classe pra cada teste, então todo teste começa com esse estoque
        var produtos = new List<Produto>
        {
            new() { CodigoProduto = 101, DescricaoProduto = "Caneta Azul", Estoque = 150 },
            new() { CodigoProduto = 105, DescricaoProduto = "Marcador de Texto Amarelo", Estoque = 90 }
        };

        _service = new EstoqueService(produtos);
    }

    [Fact]
    public void Entrada_DeveSomarNoEstoque()
    {
        var movimentacao = _service.Movimentar(101, TipoMovimentacao.Entrada, 50, "Compra de fornecedor");

        Assert.Equal(200, movimentacao.EstoqueFinal);
        Assert.Equal(200, _service.BuscarProduto(101).Estoque);
    }

    [Fact]
    public void Saida_DeveSubtrairDoEstoque()
    {
        var movimentacao = _service.Movimentar(105, TipoMovimentacao.Saida, 40, "Venda");

        Assert.Equal(50, movimentacao.EstoqueFinal);
        Assert.Equal(50, _service.BuscarProduto(105).Estoque);
    }

    [Fact]
    public void Saida_PodeZerarOEstoque()
    {
        var movimentacao = _service.Movimentar(105, TipoMovimentacao.Saida, 90, "Venda");

        Assert.Equal(0, movimentacao.EstoqueFinal);
    }

    [Fact]
    public void Saida_MaiorQueOEstoque_NaoDeveSerPermitida()
    {
        var erro = Assert.Throws<InvalidOperationException>(
            () => _service.Movimentar(105, TipoMovimentacao.Saida, 91, "Venda"));

        Assert.Contains("insuficiente", erro.Message);
        Assert.Equal(90, _service.BuscarProduto(105).Estoque);
        Assert.Empty(_service.ListarMovimentacoes());
    }

    [Fact]
    public void Entrada_QuePassaDoLimiteDoEstoque_NaoDeveSerPermitida()
    {
        // 150 + int.MaxValue estouraria o int e o estoque ficaria negativo
        var erro = Assert.Throws<InvalidOperationException>(
            () => _service.Movimentar(101, TipoMovimentacao.Entrada, int.MaxValue, "Compra de fornecedor"));

        Assert.Contains("limite", erro.Message);
        Assert.Equal(150, _service.BuscarProduto(101).Estoque);
        Assert.Empty(_service.ListarMovimentacoes());
    }

    [Fact]
    public void Entrada_AteOLimiteDoEstoque_DevePassar()
    {
        var movimentacao = _service.Movimentar(101, TipoMovimentacao.Entrada, int.MaxValue - 150, "Compra de fornecedor");

        Assert.Equal(int.MaxValue, movimentacao.EstoqueFinal);
    }

    [Fact]
    public void CadaMovimentacao_DeveTerUmIdUnico()
    {
        var primeira = _service.Movimentar(101, TipoMovimentacao.Entrada, 10, "Compra de fornecedor");
        var segunda = _service.Movimentar(101, TipoMovimentacao.Saida, 5, "Venda");
        var terceira = _service.Movimentar(105, TipoMovimentacao.Saida, 1, "Avaria");

        Assert.Equal(1, primeira.Id);
        Assert.Equal(2, segunda.Id);
        Assert.Equal(3, terceira.Id);
        Assert.Equal(3, _service.ListarMovimentacoes().Count);
    }

    [Fact]
    public void ProdutoInexistente_DeveLancarErro()
    {
        Assert.Throws<ArgumentException>(
            () => _service.Movimentar(999, TipoMovimentacao.Entrada, 10, "Compra de fornecedor"));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void QuantidadeZeroOuNegativa_DeveLancarErro(int quantidade)
    {
        Assert.Throws<ArgumentException>(
            () => _service.Movimentar(101, TipoMovimentacao.Entrada, quantidade, "Compra de fornecedor"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void DescricaoVazia_DeveLancarErro(string descricao)
    {
        Assert.Throws<ArgumentException>(
            () => _service.Movimentar(101, TipoMovimentacao.Entrada, 10, descricao));
    }
}
