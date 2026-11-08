using System.Globalization;
using System.Text;
using MiniBitcoin;

Console.OutputEncoding = Encoding.UTF8;

CultureInfo cultura;
try { cultura = CultureInfo.GetCultureInfo("pt-BR"); }
catch (CultureNotFoundException) { cultura = CultureInfo.InvariantCulture; }

var carteira = new Carteira(saldoInicialBrl: 10000m);
var cotacao = new Cotacao(350000m);
var extrato = new Extrato();
var servico = new ServicoTransacao(carteira, cotacao, extrato);

var executando = true;
while (executando)
{
    Console.WriteLine();
    Console.WriteLine("========== MINI BITCOIN ==========");
    Console.WriteLine("1 - Consultar cotação");
    Console.WriteLine("2 - Ver saldos");
    Console.WriteLine("3 - Comprar Bitcoin");
    Console.WriteLine("4 - Vender Bitcoin");
    Console.WriteLine("5 - Exibir extrato");
    Console.WriteLine("6 - Atualizar cotação");
    Console.WriteLine("0 - Sair");
    Console.Write("Escolha uma opção: ");

    var opcao = Console.ReadLine()?.Trim();
    Console.WriteLine();

    try
    {
        switch (opcao)
        {
            case "1":
                Console.WriteLine($"Cotação atual: 1 BTC = {Brl(cotacao.PrecoAtual)}");
                break;

            case "2":
                Console.WriteLine($"Saldo em BRL: {Brl(carteira.SaldoBrl)}");
                Console.WriteLine($"Saldo em BTC: {Btc(carteira.SaldoBtc)}");
                Console.WriteLine($"Patrimônio total: {Brl(extrato.CalcularPatrimonio(carteira, cotacao))}");
                break;

            case "3":
                var valorCompra = LerDecimal("Quanto deseja investir em R$? ");
                if (valorCompra is null) break;
                var btcComprado = servico.Comprar(valorCompra.Value);
                Console.WriteLine($"Compra realizada! Você recebeu {Btc(btcComprado)}.");
                break;

            case "4":
                var qtdVenda = LerDecimal("Quantos BTC deseja vender? ");
                if (qtdVenda is null) break;
                var brlRecebido = servico.Vender(qtdVenda.Value);
                Console.WriteLine($"Venda realizada! Você recebeu {Brl(brlRecebido)}.");
                break;

            case "5":
                ExibirExtrato();
                break;

            case "6":
                Console.Write("Digite o novo preço do BTC em R$ (ou ENTER para simular o mercado): ");
                var entrada = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(entrada))
                    cotacao.SimularVariacao();
                else if (TentarConverter(entrada, out var novoPreco))
                    cotacao.Atualizar(novoPreco);
                else
                {
                    Console.WriteLine("Valor inválido.");
                    break;
                }
                Console.WriteLine($"Nova cotação: 1 BTC = {Brl(cotacao.PrecoAtual)}");
                break;

            case "0":
                executando = false;
                Console.WriteLine("Até logo!");
                break;

            default:
                Console.WriteLine("Opção inválida.");
                break;
        }
    }
    catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
    {
        Console.WriteLine($"Erro: {ex.Message}");
    }
}

void ExibirExtrato()
{
    Console.WriteLine("---------------- EXTRATO ----------------");
    if (extrato.Transacoes.Count == 0)
        Console.WriteLine("Nenhuma operação realizada.");

    foreach (var t in extrato.Transacoes)
    {
        Console.WriteLine(
            $"{t.Data:dd/MM/yyyy HH:mm} | {t.Tipo,-6} | {Btc(t.QuantidadeBtc)} | {Brl(t.ValorBrl)} | cotação {Brl(t.PrecoBtc)}");
    }

    Console.WriteLine("-----------------------------------------");
    Console.WriteLine($"Total comprado: {Brl(extrato.TotalComprado)}");
    Console.WriteLine($"Total vendido:  {Brl(extrato.TotalVendido)}");
    Console.WriteLine($"Saldo BRL: {Brl(carteira.SaldoBrl)} | Saldo BTC: {Btc(carteira.SaldoBtc)}");
    Console.WriteLine($"Patrimônio total consolidado: {Brl(extrato.CalcularPatrimonio(carteira, cotacao))}");
}

decimal? LerDecimal(string mensagem)
{
    Console.Write(mensagem);
    var texto = Console.ReadLine()?.Trim() ?? string.Empty;
    if (TentarConverter(texto, out var valor))
        return valor;

    Console.WriteLine("Valor inválido. Use números, por exemplo: 150,50");
    return null;
}

bool TentarConverter(string texto, out decimal valor) =>
    decimal.TryParse(texto.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out valor);

string Brl(decimal valor) => valor.ToString("C2", cultura);

string Btc(decimal valor) => $"{valor.ToString("0.00000000", CultureInfo.InvariantCulture)} BTC";
