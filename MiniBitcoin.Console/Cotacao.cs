namespace MiniBitcoin;

/// <summary>
/// Mantém o preço atual de 1 BTC em reais (simulado).
/// </summary>
public class Cotacao
{
    public decimal PrecoAtual { get; private set; }

    public Cotacao(decimal precoInicial)
    {
        Atualizar(precoInicial);
    }

    public void Atualizar(decimal novoPreco)
    {
        if (novoPreco <= 0)
            throw new ArgumentException("A cotação do Bitcoin deve ser maior que zero.", nameof(novoPreco));
        PrecoAtual = novoPreco;
    }

    /// <summary>
    /// Simula uma oscilação de mercado entre -5% e +5%.
    /// </summary>
    public void SimularVariacao(Random? random = null)
    {
        var gerador = random ?? Random.Shared;
        var variacao = (decimal)(gerador.NextDouble() * 0.10 - 0.05);
        Atualizar(Math.Round(PrecoAtual * (1 + variacao), 2));
    }
}
