# DomusPay - Controle de Gastos Residenciais

API desenvolvida em **.NET 10 (C#)** para gerenciamento e controle de despesas e receitas residenciais, permitindo o acompanhamento do saldo líquido por pessoa e por categoria.

## Tecnologias e Padrões Utilizados

Este projeto foi construído com foco em qualidade de código, escalabilidade e boas práticas de mercado:

- **C# / .NET 10** (Web API)
- **Entity Framework Core** (ORM)
- **SQLite** (Banco de dados leve e portátil, ideal para testes)
- **Clean Architecture** (Divisão clara entre Domain, Application, Infrastructure e API)
- **Repository Pattern** (Abstração do acesso a dados)
- **FluentValidation** (Validação de DTOs via Action Filters)
- **Global Exception Handling Middleware** (Tratamento centralizado de erros retornando o padrão RFC 7807 - `ProblemDetails`)

## Regras de Negócio Destacadas

A API garante a consistência dos dados através de validações rígidas de domínio:
- Menores de 18 anos **apenas** podem registrar transações do tipo "Despesa".
- É impossível registrar uma transação com valor negativo ou zerado.
- Transações não podem utilizar categorias incompatíveis (ex: Registrar uma *Despesa* usando uma categoria com finalidade restrita à *Receita*).
- Exclusão em cascata: Ao deletar uma pessoa, todo o seu histórico de transações é removido automaticamente.

## Como Executar o Projeto (Localmente)

### Pré-requisitos
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) instalado.

### Passo a Passo

1. **Clone o repositório:**
   ```bash
   git clone [https://github.com/SEU_USUARIO/DomusPay.git](https://github.com/SEU_USUARIO/DomusPay.git)

2. **Acesse a pasta raiz do projeto**
   ```bash
   cd DomusPay

2. **Restaure os pacotes do projeto:**
   ```bash
   dotnet restore

3. **Crie o Banco de Dados (Apply Migrations):**
   ```bash
   dotnet ef database update --project DomusPay.Infrastructure --startup-project DomusPay.Api

4. **Execute a aplicação:**
   ```bash
   dotnet run --project DomusPay.Api

