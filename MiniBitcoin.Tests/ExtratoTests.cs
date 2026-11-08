namespace MiniBitcoin.Tests;

public class ExtratoTests
{
    [Fact]
    public void Extrato_NovoDeveEstarVazio()
    {
        var extrato = new Extrato();

        Assert.Empty(extrato.Transacoes);
    }

    [Fact]
    public void Comprar_DeveRegistrarTransacaoDeCompraNoExtrato()
    {
        var extrato = new Extrato();
        var servico = new ServicoTransacao(new Carteira(1000m), new Cotacao(500000m), extrato);

        servico.Comprar(1000m);

        var transacao = Assert.Single(extrato.Transacoes);
        Assert.Equal(TipoTransacao.Compra, transacao.Tipo);
        Assert.Equal(1000m, transacao.ValorBrl);
        Assert.Equal(0.002m, transacao.QuantidadeBtc);
        Assert.Equal(500000m, transacao.PrecoBtc);
    }

    [Fact]
    public void Vender_DeveRegistrarTransacaoDeVendaNoExtrato()
    {
        var extrato = new Extrato();
        var servico = new ServicoTransacao(new Carteira(0m, 0.002m), new Cotacao(500000m), extrato);

        servico.Vender(0.002m);

        var transacao = Assert.Single(extrato.Transacoes);
        Assert.Equal(TipoTransacao.Venda, transacao.Tipo);
        Assert.Equal(1000m, transacao.ValorBrl);
    }

    [Fact]
    public void OperacaoInvalida_NaoDeveSerRegistradaNoExtrato()
    {
        var extrato = new Extrato();
        var servico = new ServicoTransacao(new Carteira(100m), new Cotacao(500000m), extrato);

        Assert.Throws<InvalidOperationException>(() => servico.Comprar(500m));
        Assert.Empty(extrato.Transacoes);
    }

    [Fact]
    public void Extrato_DeveManterOrdemDasOperacoes()
    {
        var extrato = new Extrato();
        var servico = new ServicoTransacao(new Carteira(2000m), new Cotacao(500000m), extrato);

        servico.Comprar(1000m);
        servico.Vender(0.001m);

        Assert.Equal(2, extrato.Transacoes.Count);
        Assert.Equal(TipoTransacao.Compra, extrato.Transacoes[0].Tipo);
        Assert.Equal(TipoTransacao.Venda, extrato.Transacoes[1].Tipo);
        Assert.Equal(1000m, extrato.TotalComprado);
        Assert.Equal(500m, extrato.TotalVendido);
    }

    [Fact]
    public void CalcularPatrimonio_DeveSomarBrlEBtcConvertidoPelaCotacao()
    {
        var extrato = new Extrato();
        var carteira = new Carteira(1000m, 0.5m);
        var cotacao = new Cotacao(200000m);

        var patrimonio = extrato.CalcularPatrimonio(carteira, cotacao);

        Assert.Equal(101000m, patrimonio);
    }

    [Fact]
    public void CalcularPatrimonio_DeveAcompanharVariacaoDaCotacao()
    {
        var extrato = new Extrato();
        var carteira = new Carteira(0m, 1m);
        var cotacao = new Cotacao(300000m);

        cotacao.Atualizar(330000m);

        Assert.Equal(330000m, extrato.CalcularPatrimonio(carteira, cotacao));
    }
}
