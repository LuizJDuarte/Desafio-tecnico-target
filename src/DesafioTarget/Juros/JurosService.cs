namespace DesafioTarget.Juros;

public class JurosService
{
    // 2,5% ao dia sobre o valor original (juros simples). Pública porque a tela mostra a taxa
    public const decimal TaxaDiaria = 0.025m;

    // A data do cálculo vem por parâmetro em vez de usar DateTime.Today aqui dentro,
    // assim o teste consegue fixar o "hoje" e o resultado não muda de um dia pro outro
    public CalculoJuros Calcular(decimal valor, DateOnly dataVencimento, DateOnly dataCalculo)
    {
        if (valor <= 0)
        {
            throw new ArgumentException("O valor deve ser maior que zero.");
        }

        // Pagando até o dia do vencimento não tem atraso
        var diasAtraso = Math.Max(0, dataCalculo.DayNumber - dataVencimento.DayNumber);

        var juros = Math.Round(valor * TaxaDiaria * diasAtraso, 2, MidpointRounding.AwayFromZero);

        return new CalculoJuros(valor, dataVencimento, dataCalculo, diasAtraso, juros, valor + juros);
    }
}
