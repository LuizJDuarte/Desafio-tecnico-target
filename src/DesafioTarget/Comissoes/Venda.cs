namespace DesafioTarget.Comissoes;

public class Venda
{
    public string Vendedor { get; set; } = string.Empty;
    public decimal Valor { get; set; } // decimal por ser valor financeiro (double perde precisão)
}
