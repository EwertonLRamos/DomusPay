# DomusPay - Controlo de Gastos Residenciais

A palavra **DomusPay** nasce da junção de *Domus* (termo em latim que significa "casa" ou "lar") e *Pay* (do inglês, pagamento). A ideia central é fornecer um sistema completo para a gestão e controle de despesas e receitas residenciais, permitindo o acompanhamento claro do saldo líquido por pessoa e por categoria.

O projeto é dividido em duas aplicações principais: uma API RESTful robusta no Backend e uma Single Page Application (SPA) interativa no Frontend.

---

## Principais Regras de Negócio

A API garante a consistência dos dados através de validações rígidas de domínio, assegurando que o controle financeiro seja fiel à realidade:
- **Restrição de Idade:** Menores de 18 anos **apenas** podem registar transações do tipo "Despesa".
- **Validação de Valores:** É impossível registar uma transação com valor negativo ou zerado.
- **Consistência de Categorias:** As transações não podem utilizar categorias incompatíveis (ex.: Registar uma *Despesa* utilizando uma categoria com finalidade restrita a *Receita*).
- **Exclusão em Cascata:** Ao eliminar o registo de uma pessoa, todo o seu histórico de transações é removido automaticamente, evitando dados órfãos.

---

## Tecnologias, Arquitetura e Padrões

O **DomusPay** foi construído utilizando um ecossistema moderno, com foco em alta performance, manutenabilidade e escalabilidade.

### Backend (Web API)
A API foi desenvolvida seguindo os princípios de separação de responsabilidades (Clean Architecture / Arquitetura em Camadas), isolando regras de negócio, acesso a dados e pontos de entrada da aplicação.

* **Framework Principal:** .NET 10 (`net10.0`)
* **Base de Dados & ORM:** Entity Framework Core (EF Core) + SQLite (`UseSqlite`)
* **Padrões de Projeto (Design Patterns):**
  * Injeção de Dependência (DI)
  * Repository Pattern
  * Service Pattern
* **Validação de Dados:** FluentValidation + Action Filter customizado (`ValidationFilter`)
* **Tratamento de Exceções Global:** Middleware Customizado (`ExceptionHandlingMiddleware`) + respostas padronizadas no formato RFC 7807 (`ProblemDetails`)
* **Segurança e Comunicação (CORS):** Chamadas seguras oriundas das portas padrão (`localhost:5173` e `localhost:5242`)
* **Documentação Interativa:** Swagger (OpenAPI)

### Frontend (Single Page Application)
A interface foi construída com foco absoluto em velocidade de compilação, tipagem rigorosa e fluidez de navegação para o utilizador final.

* **Ecossistema Core:** React 19
* **Bundler & Build Tool:** Vite
* **Segurança de Tipos:** TypeScript
* **Comunicação Assíncrona (HTTP):** Axios
* **Feedback ao Utilizador (UI/UX):** React Toastify
* **Análise Estática e Qualidade de Código:** ESLint

---

## Como Executar o Projeto (Localmente)

### Pré-requisitos
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) instalado no seu ambiente.
- Node.js (Versão 18+ recomendada) e NPM/Yarn para executar o frontend.

### Passo a Passo

1. **Clone o repositório:**
   ```bash
   git clone [https://github.com/SEU_USUARIO/DomusPay.git](https://github.com/SEU_USUARIO/DomusPay.git)

   cd DomusPay

2. **Iniciar o Backend (API):**
   ```bash
   cd Backend
   
   dotnet restore
   
   dotnet ef database update --project DomusPay.Infrastructure --startup-project DomusPay.Api
   
   dotnet run --project DomusPay.Api

_OBS 1: A API ficará disponível em http://localhost:5242 ou https://localhost:7083 (verifique o terminal)._

3. **Iniciar o Frontend:**
   Em um novo terminal execute:
   ```bash
   cd Frontend
   
   npm install
   
   npm run dev

_OBS 2: A aplicação frontend ficará disponível em http://localhost:5173_
