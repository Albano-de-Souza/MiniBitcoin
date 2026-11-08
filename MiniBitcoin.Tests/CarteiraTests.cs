namespace MiniBitcoin.Tests;

public class CarteiraTests
{
    [Fact]
    public void Construtor_ComSaldosValidos_DeveIniciarCarteira()
    {
        var carteira = new Carteira(1000m, 0.5m);

        Assert.Equal(1000m, carteira.SaldoBrl);
        Assert.Equal(0.5m, carteira.SaldoBtc);
    }

    [Fact]
    public void Construtor_SemParametros_DeveIniciarComSaldosZerados()
    {
        var carteira = new Carteira();

        Assert.Equal(0m, carteira.SaldoBrl);
        Assert.Equal(0m, carteira.SaldoBtc);
    }

    [Fact]
    public void Construtor_ComSaldoBrlNegativo_DeveLancarExcecao()
    {
        Assert.Throws<ArgumentException>(() => new Carteira(-1m, 0m));
    }

    [Fact]
    public void Construtor_ComSaldoBtcNegativo_DeveLancarExcecao()
    {
        Assert.Throws<ArgumentException>(() => new Carteira(0m, -0.1m));
    }

    [Fact]
    public void DepositarBrl_ComValorValido_DeveAumentarSaldo()
    {
        var carteira = new Carteira(100m);

        carteira.DepositarBrl(50m);

        Assert.Equal(150m, carteira.SaldoBrl);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void DepositarBrl_ComValorZeroOuNegativo_DeveLancarExcecao(decimal valor)
    {
        var carteira = new Carteira(100m);

        Assert.Throws<ArgumentException>(() => carteira.DepositarBrl(valor));
    }

    [Fact]
    public void DebitarBrl_ComSaldoSuficiente_DeveDiminuirSaldo()
    {
        var carteira = new Carteira(100m);

        carteira.DebitarBrl(40m);

        Assert.Equal(60m, carteira.SaldoBrl);
    }

    [Fact]
    public void DebitarBrl_ComSaldoInsuficiente_DeveLancarExcecaoESaldoNaoMuda()
    {
        var carteira = new Carteira(100m);

        Assert.Throws<InvalidOperationException>(() => carteira.DebitarBrl(150m));
        Assert.Equal(100m, carteira.SaldoBrl);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void DebitarBrl_ComValorInvalido_DeveLancarExcecao(decimal valor)
    {
        var carteira = new Carteira(100m);

        Assert.Throws<ArgumentException>(() => carteira.DebitarBrl(valor));
    }

    [Fact]
    public void CreditarBtc_ComValorValido_DeveAumentarSaldoBtc()
    {
        var carteira = new Carteira();

        carteira.CreditarBtc(0.001m);

        Assert.Equal(0.001m, carteira.SaldoBtc);
    }

    [Fact]
    public void DebitarBtc_ComSaldoInsuficiente_DeveLancarExcecao()
    {
        var carteira = new Carteira(0m, 0.01m);

        Assert.Throws<InvalidOperationException>(() => carteira.DebitarBtc(0.02m));
        Assert.Equal(0.01m, carteira.SaldoBtc);
    }

    [Fact]
    public void DebitarBtc_ComSaldoSuficiente_DeveDiminuirSaldoBtc()
    {
        var carteira = new Carteira(0m, 1m);

        carteira.DebitarBtc(0.25m);

        Assert.Equal(0.75m, carteira.SaldoBtc);
    }
}
