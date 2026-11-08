namespace MiniBitcoin;

/// <summary>
/// Executa ordens de compra e venda de Bitcoin com base na cotação atual.
/// </summary>
public class ServicoTransacao
{
    private const int CasasDecimaisBtc = 8; // 1 satoshi = 0,00000001 BTC
    private const int CasasDecimaisBrl = 2;

    private readonly Carteira _carteira;
    private readonly Cotacao _cotacao;

    public ServicoTransacao(Carteira carteira, Cotacao cotacao)
    {
        _carteira = carteira ?? throw new ArgumentNullException(nameof(carteira));
        _cotacao = cotacao ?? throw new ArgumentNullException(nameof(cotacao));
    }

    /// <summary>Compra BTC gastando o valor informado em BRL. Retorna a quantidade de BTC comprada.</summary>
    public decimal Comprar(decimal valorBrl)
    {
        if (valorBrl <= 0)
            throw new ArgumentException("O valor da compra deve ser maior que zero.", nameof(valorBrl));
        if (valorBrl > _carteira.SaldoBrl)
            throw new InvalidOperationException("Saldo em BRL insuficiente para a compra.");

        var quantidadeBtc = Math.Round(valorBrl / _cotacao.PrecoAtual, CasasDecimaisBtc, MidpointRounding.ToZero);
        if (quantidadeBtc <= 0)
            throw new InvalidOperationException("Valor muito baixo: não compra nem 1 satoshi.");

        _carteira.DebitarBrl(valorBrl);
        _carteira.CreditarBtc(quantidadeBtc);
        return quantidadeBtc;
    }

    /// <summary>Vende a quantidade de BTC informada. Retorna o valor recebido em BRL.</summary>
    public decimal Vender(decimal quantidadeBtc)
    {
        if (quantidadeBtc <= 0)
            throw new ArgumentException("A quantidade de BTC deve ser maior que zero.", nameof(quantidadeBtc));
        if (quantidadeBtc > _carteira.SaldoBtc)
            throw new InvalidOperationException("Saldo em BTC insuficiente para a venda.");

        var valorBrl = Math.Round(quantidadeBtc * _cotacao.PrecoAtual, CasasDecimaisBrl, MidpointRounding.ToZero);
        if (valorBrl <= 0)
            throw new InvalidOperationException("Quantidade muito baixa: vale menos de R$ 0,01.");

        _carteira.DebitarBtc(quantidadeBtc);
        _carteira.DepositarBrl(valorBrl);
        return valorBrl;
    }
}
