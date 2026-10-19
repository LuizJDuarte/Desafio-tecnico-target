using DesafioTarget.Juros;

namespace DesafioTarget.Tests;

public class JurosServiceTests
{
    private readonly JurosService _service = new();

    // Data fixa pra o teste não depender do dia em que roda
    private readonly DateOnly _hoje = new(2026, 10, 19);

    [Fact]
    public void ComAtraso_DeveCobrarDoisEMeioPorCentoPorDia()
    {
        var calculo = _service.Calcular(1000m, new DateOnly(2026, 10, 10), _hoje);

        Assert.Equal(9, calculo.DiasAtraso);
        Assert.Equal(225.00m, calculo.ValorJuros); // 1000 x 2,5% x 9
        Assert.Equal(1225.00m, calculo.ValorAtualizado);
    }

    [Fact]
    public void AtrasoVirandoOMes_DeveContarOsDiasCorridos()
    {
        var calculo = _service.Calcular(200m, new DateOnly(2026, 9, 30), _hoje);

        Assert.Equal(19, calculo.DiasAtraso);
        Assert.Equal(95.00m, calculo.ValorJuros);
    }

    [Theory]
    [InlineData(0)] // vence hoje
    [InlineData(5)] // vence daqui a 5 dias
    public void SemAtraso_NaoTemJuros(int diasAteOVencimento)
    {
        var calculo = _service.Calcular(1000m, _hoje.AddDays(diasAteOVencimento), _hoje);

        Assert.Equal(0, calculo.DiasAtraso);
        Assert.Equal(0m, calculo.ValorJuros);
        Assert.Equal(1000m, calculo.ValorAtualizado);
    }

    [Fact]
    public void Juros_DeveUsarArredondamentoComercial()
    {
        // 100,20 x 2,5% = 2,505 -> 2,51 (o padrão do Math.Round, arredondamento bancário, daria 2,50)
        var calculo = _service.Calcular(100.20m, _hoje.AddDays(-1), _hoje);

        Assert.Equal(2.51m, calculo.ValorJuros);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-50)]
    public void ValorZeroOuNegativo_DeveLancarErro(int valor)
    {
        Assert.Throws<ArgumentException>(() => _service.Calcular(valor, _hoje.AddDays(-3), _hoje));
    }
}
