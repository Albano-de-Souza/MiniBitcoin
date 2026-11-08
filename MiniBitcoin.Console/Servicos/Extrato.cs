using MiniBitcoin.Dominio;

namespace MiniBitcoin.Servicos;

/// <summary>
/// Histórico de operações e cálculo do patrimônio consolidado.
/// </summary>
public class Extrato
{
    private readonly List<Transacao> _transacoes = new();

    public IReadOnlyList<Transacao> Transacoes => _transacoes.AsReadOnly();

    public decimal TotalComprado =>
        _transacoes.Where(t => t.Tipo == TipoTransacao.Compra).Sum(t => t.ValorBrl);

    public decimal TotalVendido =>
        _transacoes.Where(t => t.Tipo == TipoTransacao.Venda).Sum(t => t.ValorBrl);

    public void Registrar(Transacao transacao)
    {
        ArgumentNullException.ThrowIfNull(transacao);
        _transacoes.Add(transacao);
    }

    /// <summary>Patrimônio total em BRL = saldo BRL + (saldo BTC × cotação atual).</summary>
    public decimal CalcularPatrimonio(Carteira carteira, Cotacao cotacao)
    {
        ArgumentNullException.ThrowIfNull(carteira);
        ArgumentNullException.ThrowIfNull(cotacao);
        return Math.Round(carteira.SaldoBrl + carteira.SaldoBtc * cotacao.PrecoAtual, 2);
    }
}
