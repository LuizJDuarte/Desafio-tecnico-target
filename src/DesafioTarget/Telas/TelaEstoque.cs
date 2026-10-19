using DesafioTarget.Estoques;
using DesafioTarget.Util;

namespace DesafioTarget.Telas;

public class TelaEstoque
{
    private readonly EstoqueService _estoqueService;

    public TelaEstoque(EstoqueService estoqueService)
    {
        _estoqueService = estoqueService;
    }

    public void Exibir()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("----- Movimentação de estoque -----");
            ListarProdutos();

            Console.WriteLine();
            Console.WriteLine("1 - Lançar entrada");
            Console.WriteLine("2 - Lançar saída");
            Console.WriteLine("3 - Ver movimentações");
            Console.WriteLine("0 - Voltar");

            var opcao = ConsoleHelper.LerTexto("Escolha uma opção: ");

            switch (opcao)
            {
                case "1":
                    LancarMovimentacao(TipoMovimentacao.Entrada);
                    break;
                case "2":
                    LancarMovimentacao(TipoMovimentacao.Saida);
                    break;
                case "3":
                    ListarMovimentacoes();
                    break;
                case "0":
                    return;
                default:
                    ConsoleHelper.Erro("Opção inválida.");
                    break;
            }
        }
    }

    private void ListarProdutos()
    {
        Console.WriteLine($"{"Código",-8}{"Produto",-28}{"Estoque",8}");

        foreach (var produto in _estoqueService.ListarProdutos())
        {
            Console.WriteLine($"{produto.CodigoProduto,-8}{produto.DescricaoProduto,-28}{produto.Estoque,8}");
        }
    }

    private void LancarMovimentacao(TipoMovimentacao tipo)
    {
        Console.WriteLine();
        Console.WriteLine($"Nova {NomeTipo(tipo).ToLower()}");

        try
        {
            var codigo = ConsoleHelper.LerInteiro("Código do produto: ");
            var produto = _estoqueService.BuscarProduto(codigo);
            Console.WriteLine($"{produto.DescricaoProduto} | estoque atual: {produto.Estoque}");

            var quantidade = ConsoleHelper.LerInteiro("Quantidade: ");
            var descricao = ConsoleHelper.LerTexto("Descrição da movimentação (ex: Compra de fornecedor, Venda, Devolução): ");

            var movimentacao = _estoqueService.Movimentar(codigo, tipo, quantidade, descricao);

            Console.WriteLine();
            ConsoleHelper.Sucesso($"Movimentação #{movimentacao.Id} registrada: {NomeTipo(tipo)} de {quantidade} un. ({movimentacao.Descricao})");
            Console.WriteLine($"Estoque final de {produto.DescricaoProduto}: {movimentacao.EstoqueFinal}");
        }
        catch (ArgumentException ex)
        {
            ConsoleHelper.Erro(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            ConsoleHelper.Erro(ex.Message);
        }
    }

    private void ListarMovimentacoes()
    {
        var movimentacoes = _estoqueService.ListarMovimentacoes();

        Console.WriteLine();

        if (movimentacoes.Count == 0)
        {
            Console.WriteLine("Nenhuma movimentação lançada ainda.");
            return;
        }

        Console.WriteLine($"{"Id",-5}{"Data/Hora",-18}{"Produto",-28}{"Tipo",-9}{"Qtd",6}{"Estoque final",15}  Descrição");

        foreach (var movimentacao in movimentacoes)
        {
            var produto = _estoqueService.BuscarProduto(movimentacao.CodigoProduto);
            var dataHora = ConsoleHelper.FormatarDataHora(movimentacao.DataHora);

            Console.WriteLine(
                $"{movimentacao.Id,-5}{dataHora,-18}{produto.DescricaoProduto,-28}{NomeTipo(movimentacao.Tipo),-9}" +
                $"{movimentacao.Quantidade,6}{movimentacao.EstoqueFinal,15}  {movimentacao.Descricao}");
        }
    }

    private static string NomeTipo(TipoMovimentacao tipo)
    {
        return tipo == TipoMovimentacao.Entrada ? "Entrada" : "Saída";
    }
}
