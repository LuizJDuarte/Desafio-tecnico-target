namespace DesafioTarget.Estoques;

public class EstoqueService
{
    private readonly List<Produto> _produtos;
    private readonly List<Movimentacao> _movimentacoes = new();

    // Faz o papel de um IDENTITY no banco: cada movimentação pega o próximo número
    private int _ultimoId;

    public EstoqueService(List<Produto> produtos)
    {
        _produtos = produtos;
    }

    public IReadOnlyList<Produto> ListarProdutos()
    {
        return _produtos;
    }

    public IReadOnlyList<Movimentacao> ListarMovimentacoes()
    {
        return _movimentacoes;
    }

    public Produto BuscarProduto(int codigoProduto)
    {
        var produto = _produtos.FirstOrDefault(p => p.CodigoProduto == codigoProduto);

        if (produto == null)
        {
            throw new ArgumentException($"Produto {codigoProduto} não encontrado.");
        }

        return produto;
    }

    public Movimentacao Movimentar(int codigoProduto, TipoMovimentacao tipo, int quantidade, string descricao)
    {
        var produto = BuscarProduto(codigoProduto);

        if (quantidade <= 0)
        {
            throw new ArgumentException("A quantidade deve ser maior que zero.");
        }

        if (string.IsNullOrWhiteSpace(descricao))
        {
            throw new ArgumentException("Informe uma descrição para a movimentação.");
        }

        // Saída não pode deixar o estoque negativo
        if (tipo == TipoMovimentacao.Saida && quantidade > produto.Estoque)
        {
            throw new InvalidOperationException(
                $"Estoque insuficiente de {produto.DescricaoProduto}. Disponível: {produto.Estoque}, solicitado: {quantidade}.");
        }

        // Entrada não pode passar do limite do int, senão a soma estoura e o estoque vira negativo sem dar erro
        if (tipo == TipoMovimentacao.Entrada && quantidade > int.MaxValue - produto.Estoque)
        {
            throw new InvalidOperationException(
                $"A entrada passa do limite de estoque de {produto.DescricaoProduto}. Máximo que ainda cabe: {int.MaxValue - produto.Estoque}.");
        }

        if (tipo == TipoMovimentacao.Entrada)
        {
            produto.Estoque += quantidade;
        }
        else
        {
            produto.Estoque -= quantidade;
        }

        _ultimoId++;

        var movimentacao = new Movimentacao
        {
            Id = _ultimoId,
            CodigoProduto = produto.CodigoProduto,
            Tipo = tipo,
            Quantidade = quantidade,
            Descricao = descricao.Trim(),
            DataHora = DateTime.Now,
            EstoqueFinal = produto.Estoque
        };

        _movimentacoes.Add(movimentacao);

        return movimentacao;
    }
}
