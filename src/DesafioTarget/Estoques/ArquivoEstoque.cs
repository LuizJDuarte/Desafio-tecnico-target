namespace DesafioTarget.Estoques;

// Raiz do estoque.json: { "estoque": [ ... ] }
public class ArquivoEstoque
{
    public List<Produto> Estoque { get; set; } = new();
}
