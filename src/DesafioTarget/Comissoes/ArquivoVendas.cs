namespace DesafioTarget.Comissoes;

// Raiz do vendas.json: { "vendas": [ ... ] }
public class ArquivoVendas
{
    public List<Venda> Vendas { get; set; } = new();
}
