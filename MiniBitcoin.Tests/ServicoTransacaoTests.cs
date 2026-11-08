using MiniBitcoin.Dominio;
using MiniBitcoin.Servicos;

namespace MiniBitcoin.Tests;

public class ServicoTransacaoTests
{
    [Fact]
    public void Cotacao_ComPrecoZeroOuNegativo_DeveLancarExcecao()
    {
        Assert.Throws<ArgumentException>(() => new Cotacao(0m));
        Assert.Throws<ArgumentException>(() => new Cotacao(-100m));
    }

    [Fact]
    public void Cotacao_Atualizar_DeveAlterarPreco()
    {
        var cotacao = new Cotacao(300000m);

        cotacao.Atualizar(350000m);

        Assert.Equal(350000m, cotacao.PrecoAtual);
    }

    [Fact]
    public void Comprar_ComSaldoSuficiente_DeveConverterBrlEmBtc()
    {
        var carteira = new Carteira(1000m);
        var servico = new ServicoTransacao(carteira, new Cotacao(500000m));

        var btcComprado = servico.Comprar(1000m);

        Assert.Equal(0.002m, btcComprado);
        Assert.Equal(0m, carteira.SaldoBrl);
        Assert.Equal(0.002m, carteira.SaldoBtc);
    }

    [Fact]
    public void Comprar_FracaoDeBitcoin_DeveFuncionar()
    {
        var carteira = new Carteira(500m);
        var servico = new ServicoTransacao(carteira, new Cotacao(400000m));

        var btcComprado = servico.Comprar(100m);

        Assert.Equal(0.00025m, btcComprado);
        Assert.Equal(400m, carteira.SaldoBrl);
    }

    [Fact]
    public void Comprar_ComSaldoInsuficiente_DeveLancarExcecaoESaldosNaoMudam()
    {
        var carteira = new Carteira(100m);
        var servico = new ServicoTransacao(carteira, new Cotacao(500000m));

        Assert.Throws<InvalidOperationException>(() => servico.Comprar(200m));
        Assert.Equal(100m, carteira.SaldoBrl);
        Assert.Equal(0m, carteira.SaldoBtc);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-50)]
    public void Comprar_ComValorInvalido_DeveLancarExcecao(decimal valor)
    {
        var servico = new ServicoTransacao(new Carteira(1000m), new Cotacao(500000m));

        Assert.Throws<ArgumentException>(() => servico.Comprar(valor));
    }

    [Fact]
    public void Vender_ComSaldoBtcSuficiente_DeveConverterBtcEmBrl()
    {
        var carteira = new Carteira(0m, 0.002m);
        var servico = new ServicoTransacao(carteira, new Cotacao(500000m));

        var brlRecebido = servico.Vender(0.002m);

        Assert.Equal(1000m, brlRecebido);
        Assert.Equal(1000m, carteira.SaldoBrl);
        Assert.Equal(0m, carteira.SaldoBtc);
    }

    [Fact]
    public void Vender_ComSaldoBtcInsuficiente_DeveLancarExcecao()
    {
        var carteira = new Carteira(0m, 0.001m);
        var servico = new ServicoTransacao(carteira, new Cotacao(500000m));

        Assert.Throws<InvalidOperationException>(() => servico.Vender(0.01m));
        Assert.Equal(0.001m, carteira.SaldoBtc);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-0.5)]
    public void Vender_ComQuantidadeInvalida_DeveLancarExcecao(decimal quantidade)
    {
        var servico = new ServicoTransacao(new Carteira(0m, 1m), new Cotacao(500000m));

        Assert.Throws<ArgumentException>(() => servico.Vender(quantidade));
    }

    [Fact]
    public void Vender_AposMudancaDeCotacao_DeveUsarPrecoAtual()
    {
        var carteira = new Carteira(0m, 1m);
        var cotacao = new Cotacao(300000m);
        var servico = new ServicoTransacao(carteira, cotacao);

        cotacao.Atualizar(400000m);
        var brlRecebido = servico.Vender(0.5m);

        Assert.Equal(200000m, brlRecebido);
    }
}
