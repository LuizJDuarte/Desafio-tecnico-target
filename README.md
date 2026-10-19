# Desafio Técnico Target Sistemas

[![CI](https://github.com/LuizJDuarte/Desafio-tecnico-target/actions/workflows/ci.yml/badge.svg)](https://github.com/LuizJDuarte/Desafio-tecnico-target/actions/workflows/ci.yml)

Aplicação de console em C# com as três questões do desafio técnico da vaga de Desenvolvedor de Sistemas Jr: comissão dos vendedores, movimentação de estoque e cálculo de juros por atraso.

## Funcionalidades

- **Comissão dos vendedores:** lê as vendas do `vendas.json` e calcula a comissão de cada vendedor. Venda abaixo de R$ 100,00 não gera comissão, abaixo de R$ 500,00 gera 1% e a partir de R$ 500,00 gera 5%.
- **Movimentação de estoque:** lança entradas e saídas dos produtos do `estoque.json`. Cada movimentação recebe um número identificador único e uma descrição, e ao final o programa mostra a quantidade final em estoque do produto movimentado. Também é possível consultar as movimentações já lançadas.
- **Juros por atraso:** a partir de um valor e de uma data de vencimento, calcula os juros na data de hoje com taxa de 2,5% ao dia.

## Tecnologias

- C# 14 e .NET 10
- System.Text.Json para ler os arquivos JSON
- xUnit para os testes
- GitHub Actions para rodar o build e os testes a cada push

## Estrutura do projeto

```text
src/DesafioTarget
├── Comissoes/   regra de comissão e modelos de venda
├── Estoques/    produtos, movimentações e regras de estoque
├── Juros/       cálculo dos juros por atraso
├── Telas/       interação com o usuário no console
├── Util/        leitura dos JSONs e funções de entrada e saída do console
├── Dados/       vendas.json e estoque.json do desafio
└── Program.cs   menu principal

tests/DesafioTarget.Tests
```

As regras de negócio ficam nos services (`ComissaoService`, `EstoqueService` e `JurosService`), que não usam o `Console`. As telas só leem o que o usuário digita, chamam o service e mostram o resultado. Com essa separação as regras são testadas diretamente, sem passar pela interface.

## Como executar

### Pré-requisitos

- .NET 10 SDK

```bash
git clone https://github.com/LuizJDuarte/Desafio-tecnico-target.git
cd Desafio-tecnico-target
dotnet run --project src/DesafioTarget
```

Também dá para abrir o `DesafioTarget.sln` no Visual Studio ou no Rider e rodar o projeto `DesafioTarget`.

O programa abre um menu com as três questões:

```text
===== Desafio Técnico Target Sistemas =====
1 - Comissão dos vendedores
2 - Movimentação de estoque
3 - Cálculo de juros por atraso
0 - Sair
```

## Testes

```bash
dotnet test
```

Os testes cobrem as faixas de comissão (incluindo os limites de R$ 100,00 e R$ 500,00), o resultado com os dados do desafio, as validações da movimentação de estoque (como saída maior que o saldo e entrada que passaria do limite do `int`) e o cálculo dos juros com uma data fixa.

O workflow em `.github/workflows/ci.yml` roda o build e os testes a cada push e pull request na `main`. O selo no topo deste README mostra o resultado da última execução.

## Exemplos de execução

### Comissão dos vendedores

| Vendedor | Vendas | Total vendido | Comissão |
| --- | --- | --- | --- |
| João Silva | 10 | R$ 10.754,70 | R$ 495,68 |
| Maria Souza | 9 | R$ 9.874,30 | R$ 465,95 |
| Carlos Oliveira | 8 | R$ 7.928,35 | R$ 379,37 |
| Ana Lima | 9 | R$ 8.763,95 | R$ 404,98 |

Total de comissões: R$ 1.745,98

### Movimentação de estoque

Entrada de 30 canetas:

```text
----- Movimentação de estoque -----
Código  Produto                      Estoque
101     Caneta Azul                      150
102     Caderno Universitário             75
103     Borracha Branca                  200
104     Lápis Preto HB                   320
105     Marcador de Texto Amarelo         90

1 - Lançar entrada
2 - Lançar saída
3 - Ver movimentações
0 - Voltar
Escolha uma opção: 1

Nova entrada
Código do produto: 101
Caneta Azul | estoque atual: 150
Quantidade: 30
Descrição da movimentação (ex: Compra de fornecedor, Venda, Devolução): Compra de fornecedor

Movimentação #1 registrada: Entrada de 30 un. (Compra de fornecedor)
Estoque final de Caneta Azul: 180
```

Saída maior que o estoque disponível, que é recusada:

```text
Nova saída
Código do produto: 105
Marcador de Texto Amarelo | estoque atual: 90
Quantidade: 100
Descrição da movimentação (ex: Compra de fornecedor, Venda, Devolução): Venda
Erro: Estoque insuficiente de Marcador de Texto Amarelo. Disponível: 90, solicitado: 100.
```

### Juros por atraso

Exemplo executado em 19/10/2026:

```text
----- Cálculo de juros por atraso -----
Valor (ex: 1500,00): 1000
Data de vencimento (dd/mm/aaaa): 10/10/2026

Valor original:      R$ 1.000,00
Vencimento:          10/10/2026
Data do cálculo:     19/10/2026
Dias em atraso:      9
Taxa:                2,5% ao dia
Juros:               R$ 225,00
Valor atualizado:    R$ 1.225,00
```

## Decisões e premissas

- Valores em dinheiro usam `decimal`, que guarda os centavos sem as imprecisões do `double`.
- A comissão de cada venda é calculada sem arredondamento e só o total do vendedor é arredondado para 2 casas. Arredondando venda por venda, todos os vendedores ficariam com 1 centavo a mais (João Silva com R$ 495,69, por exemplo).
- O arredondamento usado é o comercial (`MidpointRounding.AwayFromZero`). O padrão do `Math.Round` no C# é o arredondamento bancário, que transforma 2,505 em 2,50.
- Uma saída maior que o estoque disponível é recusada, então o estoque nunca fica negativo. Uma entrada que faria o estoque passar do limite do `int` também é recusada, porque a soma estouraria e o saldo viraria negativo sem dar erro.
- O identificador da movimentação é sequencial, como um `IDENTITY` no banco. As movimentações ficam em memória enquanto o programa está aberto e o `estoque.json` não é alterado.
- Os juros são simples: 2,5% sobre o valor original para cada dia corrido de atraso, sem considerar fins de semana e feriados. Até o dia do vencimento não há juros.
- O `JurosService` recebe a data do cálculo por parâmetro em vez de usar `DateTime.Today` dentro da regra. O programa passa a data de hoje e os testes passam uma data fixa.
- As telas mostram a regra de comissão e a taxa de juros a partir das constantes dos services, então o texto nunca fica diferente do cálculo.
- Erros inesperados são tratados no menu principal e o programa continua rodando, em vez de fechar mostrando o stack trace.

## Melhorias futuras

- Guardar produtos e movimentações no SQL Server, com o id da movimentação em `IDENTITY` e a atualização do estoque dentro de uma transação, para duas movimentações simultâneas não se atropelarem.
- Expor as três regras em uma API REST com ASP.NET Core, reaproveitando os mesmos services.
- Permitir o estorno de uma movimentação com um lançamento contrário, em vez de apagar o registro, para manter o histórico do estoque.

---

Desenvolvido por [Luiz Duarte](https://github.com/LuizJDuarte).
