namespace DesafioTarget.Juros;

public record CalculoJuros(
    decimal ValorOriginal,
    DateOnly DataVencimento,
    DateOnly DataCalculo,
    int DiasAtraso,
    decimal ValorJuros,
    decimal ValorAtualizado);
