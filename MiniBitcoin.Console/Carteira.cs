namespace MiniBitcoin;

/// <summary>
/// Carteira com saldo em reais (BRL) e em Bitcoin (BTC).
/// Nenhuma operação pode deixar os saldos negativos.
/// </summary>
public class Carteira
{
    public decimal SaldoBrl { get; private set; }
    public decimal SaldoBtc { get; private set; }

    public Carteira(decimal saldoInicialBrl = 0m, decimal saldoInicialBtc = 0m)
    {
        if (saldoInicialBrl < 0)
            throw new ArgumentException("O saldo inicial em BRL não pode ser negativo.", nameof(saldoInicialBrl));
        if (saldoInicialBtc < 0)
            throw new ArgumentException("O saldo inicial em BTC não pode ser negativo.", nameof(saldoInicialBtc));

        SaldoBrl = saldoInicialBrl;
        SaldoBtc = saldoInicialBtc;
    }

    public void DepositarBrl(decimal valor)
    {
        ValidarValorPositivo(valor);
        SaldoBrl += valor;
    }

    public void DebitarBrl(decimal valor)
    {
        ValidarValorPositivo(valor);
        if (valor > SaldoBrl)
            throw new InvalidOperationException("Saldo em BRL insuficiente.");
        SaldoBrl -= valor;
    }

    public void CreditarBtc(decimal quantidade)
    {
        ValidarValorPositivo(quantidade);
        SaldoBtc += quantidade;
    }

    public void DebitarBtc(decimal quantidade)
    {
        ValidarValorPositivo(quantidade);
        if (quantidade > SaldoBtc)
            throw new InvalidOperationException("Saldo em BTC insuficiente.");
        SaldoBtc -= quantidade;
    }

    private static void ValidarValorPositivo(decimal valor)
    {
        if (valor <= 0)
            throw new ArgumentException("O valor da operação deve ser maior que zero.", nameof(valor));
    }
}
