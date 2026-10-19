using System.Text;
using DesafioTarget.Comissoes;
using DesafioTarget.Estoques;
using DesafioTarget.Juros;
using DesafioTarget.Telas;
using DesafioTarget.Util;

// Sem isso a acentuação sai quebrada em alguns terminais do Windows
Console.OutputEncoding = Encoding.UTF8;

List<Venda> vendas;
List<Produto> produtos;

try
{
    vendas = LeitorJson.Ler<ArquivoVendas>("vendas.json").Vendas;
    produtos = LeitorJson.Ler<ArquivoEstoque>("estoque.json").Estoque;
}
catch (Exception ex)
{
    ConsoleHelper.Erro($"Não foi possível carregar os dados do desafio. {ex.Message}");
    return;
}

var telaComissoes = new TelaComissoes(new ComissaoService(), vendas);
var telaEstoque = new TelaEstoque(new EstoqueService(produtos));
var telaJuros = new TelaJuros(new JurosService());

while (true)
{
    Console.WriteLine();
    Console.WriteLine("===== Desafio Técnico Target Sistemas =====");
    Console.WriteLine("1 - Comissão dos vendedores");
    Console.WriteLine("2 - Movimentação de estoque");
    Console.WriteLine("3 - Cálculo de juros por atraso");
    Console.WriteLine("0 - Sair");

    var opcao = ConsoleHelper.LerTexto("Escolha uma opção: ");

    try
    {
        switch (opcao)
        {
            case "1":
                telaComissoes.Exibir();
                break;
            case "2":
                telaEstoque.Exibir();
                break;
            case "3":
                telaJuros.Exibir();
                break;
            case "0":
                Console.WriteLine("Até mais!");
                return;
            default:
                ConsoleHelper.Erro("Opção inválida.");
                break;
        }
    }
    catch (OverflowException)
    {
        ConsoleHelper.Erro("O valor informado é grande demais para o cálculo.");
    }
    catch (Exception ex)
    {
        // Qualquer erro que escapar das telas cai aqui, assim o programa não fecha no meio do uso
        ConsoleHelper.Erro($"Algo inesperado aconteceu: {ex.Message}");
    }
}
