# Desafio Técnico Target Sistemas

Aplicação de console em C# com as três questões do desafio técnico da vaga de Desenvolvedor de Sistemas Jr: comissão dos vendedores, movimentação de estoque e cálculo de juros por atraso.

## Funcionalidades

- **Comissão dos vendedores:** lê as vendas do `vendas.json` e calcula a comissão de cada vendedor. Venda abaixo de R$ 100,00 não gera comissão, abaixo de R$ 500,00 gera 1% e a partir de R$ 500,00 gera 5%.
- **Movimentação de estoque:** lança entradas e saídas dos produtos do `estoque.json`. Cada movimentação recebe um número identificador único e uma descrição, e ao final o programa mostra a quantidade final em estoque do produto movimentado. Também é possível consultar as movimentações já lançadas.
- **Juros por atraso:** a partir de um valor e de uma data de vencimento, calcula os juros na data de hoje com taxa de 2,5% ao dia.

## Tecnologias

- C# 14 e .NET 10
- System.Text.Json para ler os arquivos JSON
- xUnit para os testes

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

Os testes cobrem as faixas de comissão (incluindo os limites de R$ 100,00 e R$ 500,00), o resultado com os dados do desafio, as validações da movimentação de estoque e o cálculo dos juros com uma data fixa.

## Resultado das comissões

| Vendedor | Vendas | Total vendido | Comissão |
| --- | --- | --- | --- |
| João Silva | 10 | R$ 10.754,70 | R$ 495,68 |
| Maria Souza | 9 | R$ 9.874,30 | R$ 465,95 |
| Carlos Oliveira | 8 | R$ 7.928,35 | R$ 379,37 |
| Ana Lima | 9 | R$ 8.763,95 | R$ 404,98 |

Total de comissões: R$ 1.745,98

## Decisões e premissas

- Valores em dinheiro usam `decimal`, que guarda os centavos sem as imprecisões do `double`.
- A comissão de cada venda é calculada sem arredondamento e só o total do vendedor é arredondado para 2 casas. Arredondando venda por venda, todos os vendedores ficariam com 1 centavo a mais (João Silva com R$ 495,69, por exemplo).
- O arredondamento usado é o comercial (`MidpointRounding.AwayFromZero`). O padrão do `Math.Round` no C# é o arredondamento bancário, que transforma 2,505 em 2,50.
- Uma saída maior que o estoque disponível é recusada, então o estoque nunca fica negativo.
- O identificador da movimentação é sequencial, como um `IDENTITY` no banco. As movimentações ficam em memória enquanto o programa está aberto e o `estoque.json` não é alterado.
- Os juros são simples: 2,5% sobre o valor original para cada dia corrido de atraso, sem considerar fins de semana e feriados. Até o dia do vencimento não há juros.
- O `JurosService` recebe a data do cálculo por parâmetro em vez de usar `DateTime.Today` dentro da regra. O programa passa a data de hoje e os testes passam uma data fixa.

---

Desenvolvido por [Luiz Duarte](https://github.com/LuizJDuarte).
