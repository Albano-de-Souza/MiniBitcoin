# MiniBitcoin

Aplicação de console em **C# / .NET 8** que simula uma carteira de Bitcoin: compra e venda de BTC usando saldo em reais (BRL), cotação simulada e extrato de operações com patrimônio consolidado.

Projeto desenvolvido para a disciplina **Gestão e Qualidade de Software**, seguindo TDD básico (testes escritos antes da implementação) e o padrão **Conventional Commits**.

## Funcionalidades

- **Carteira (Wallet):** saldo em BRL e em BTC, sem permitir saldos negativos.
- **Cotação:** preço atual do Bitcoin, atualizável manualmente ou por simulação de mercado (variação de -5% a +5%).
- **Compra e venda:** ordens de compra/venda com validação de saldo disponível (BTC com precisão de 8 casas = 1 satoshi).
- **Extrato:** histórico de compras e vendas, totais e patrimônio total consolidado em BRL.

## Arquitetura

```
MiniBitcoin/
├── MiniBitcoin.sln
├── MiniBitcoin.Console/          # Aplicação (console)
│   ├── Dominio/
│   │   ├── Carteira.cs           # Saldos BRL/BTC e regras de débito/crédito
│   │   ├── Cotacao.cs            # Preço do BTC (atualizar / simular variação)
│   │   └── Transacao.cs          # Registro de operação + enum TipoTransacao
│   ├── Servicos/
│   │   ├── ServicoTransacao.cs   # Lógica de compra e venda
│   │   └── Extrato.cs            # Histórico e cálculo do patrimônio
│   └── Program.cs                # Menu interativo
└── MiniBitcoin.Tests/            # Testes unitários (xUnit)
    ├── CarteiraTests.cs
    ├── ServicoTransacaoTests.cs
    └── ExtratoTests.cs
```

- **Dominio:** entidades e regras de negócio básicas.
- **Servicos:** orquestram as operações entre carteira, cotação e extrato.
- **Program.cs:** apenas interface com o usuário; não contém regra de negócio.

## Pré-requisitos

- [.NET SDK 8](https://dotnet.microsoft.com/download) ou superior

## Como executar

Na pasta raiz do projeto:

```bash
# Restaurar dependências e compilar
dotnet build

# Rodar os testes unitários
dotnet test

# Executar a aplicação
dotnet run --project MiniBitcoin.Console/MiniBitcoin.Console.csproj
```

## Menu da aplicação

| Opção | Ação |
|-------|------|
| 1 | Consultar cotação |
| 2 | Ver saldos e patrimônio |
| 3 | Comprar Bitcoin (informando o valor em R$) |
| 4 | Vender Bitcoin (informando a quantidade em BTC) |
| 5 | Exibir extrato |
| 6 | Atualizar cotação (valor manual ou ENTER para simular) |
| 0 | Sair |

A carteira começa com **R$ 10.000,00** e a cotação inicial é **R$ 350.000,00** por BTC.

## Padrão de commits

| Tipo | Uso |
|------|-----|
| `feat` | Nova funcionalidade |
| `fix` | Correção de bug |
| `test` | Criação/alteração de testes |
| `refactor` | Melhoria de código sem mudar comportamento |
| `docs` | Documentação |
| `chore` | Configuração e estrutura |

Para ver o histórico: `git log --oneline`

## Autor

Albano de Souza — RA 32516336
