namespace MiniBitcoin.Dominio;

public enum TipoTransacao
{
    Compra,
    Venda
}

/// <summary>
/// Registro imutável de uma operação de compra ou venda.
/// </summary>
public record Transacao(
    TipoTransacao Tipo,
    decimal QuantidadeBtc,
    decimal ValorBrl,
    decimal PrecoBtc,
    DateTime Data);
