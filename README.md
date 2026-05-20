# 🏛️ BureauHexagonal — Bureau de Dados com Arquitetura Hexagonal & DDD

[![.NET 8](https://img.shields.io/badge/.NET-8.0-blueviolet.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![Architecture](https://img.shields.io/badge/Architecture-Hexagonal%20%2F%20DDD-brightgreen.svg)]()
[![Database](https://img.shields.io/badge/Database-PostgreSQL-blue.svg)](https://www.postgresql.org/)
[![Tests](https://img.shields.io/badge/Tests-xUnit%20%2F%20FluentAssertions-green.svg)]()

O **BureauHexagonal** é um sistema corporativo robusto para consulta, consolidação e normalização de informações de bureaus de dados (como informações de CEP vindas de provedores externos como *ViaCep* e *BrasilApi*). A aplicação fornece APIs HTTP padronizadas e otimizadas para consumo interno ou externo, com foco em resiliência, cache em banco de dados e alto desempenho.

O projeto foi construído seguindo rigorosamente os princípios de **Domain-Driven Design (DDD)**, **Arquitetura Hexagonal (Ports & Adapters)** e os princípios do **SOLID**.

---

## 🚀 Tecnologias Utilizadas

- **Runtime & SDK**: .NET 8.0 C#
- **Banco de Dados**: PostgreSQL (com persistência via Entity Framework Core)
- **Gateways**: Integrações HTTP resilientes com provedores externos (ViaCep e BrasilApi)
- **Padrão de Resposta**: *Result Pattern* com a interface `IOperation` para tratamento limpo de fluxos de erro e sucesso sem uso excessivo de exceções
- **Testes**: xUnit, Moq, FluentAssertions
- **Estruturação de Logs**: Serilog

---

## 🏛️ Arquitetura e Estrutura do Projeto

O projeto é dividido em quatro camadas principais (arquitetura de portas e adaptadores) que protegem as regras de negócio de acoplamento direto com tecnologias e frameworks externos:

```text
/src
  ├── BureauHexagonal.Api            # Camada de Apresentação (Controllers, Middlewares, Injeção de Dependências)
  ├── BureauHexagonal.Application    # Casos de Uso (Use Cases), DTOs de entrada e saída, Validadores de fluxo
  ├── BureauHexagonal.Core           # O Coração do Domínio (Entidades, Enums, Regras de negócio, Interfaces/Ports)
  └── BureauHexagonal.Infrastructure # Implementação dos Adapters (PostgreSQL, EF Core, HTTP Clients para APIs externas)
/tests
  ├── BureauHexagonal.Test.Unit      # Testes Unitários de alta cobertura (Domínio, Casos de Uso, Validadores)
  └── BureauHexagonal.Test.Integration # Testes de Integração automatizados para fluxos de ponta a ponta
```

### Detalhamento das Camadas

*   **`BureauHexagonal.Core (Domain)`**:
    Contém as entidades de negócio principais (como a `BureauEntity`), que protegem suas invariantes e encapsulam as regras fundamentais do domínio. Não possui nenhuma dependência de bibliotecas externas ou frameworks de persistência. Declara as **Portas** (`Ports` / Interfaces) para repositórios e serviços externos de forma desacoplada.
    
*   **`BureauHexagonal.Application`**:
    Orquestra as operações do negócio por meio de casos de uso (`UseCases`), que implementam a interface `IUseCase<TInput>`. Cada caso de uso possui uma validação associada (`IUseCaseValidation<TInput>`) e gerencia fluxos transacionais por meio da porta de `IUnitOfWork`.
    
*   **`BureauHexagonal.Infrastructure`**:
    Contém a implementação física e técnica de todas as portas. Isso inclui o acesso ao PostgreSQL usando Entity Framework Core, repositórios concretos, o mecanismo de transações (`UnitOfWork`) e adaptadores HTTP para os bureaus de dados (ViaCep/BrasilApi).
    
*   **`BureauHexagonal.Api`**:
    Expeõe as APIs Web HTTP REST. É responsável por receber as requisições HTTP, injetar dependências nas camadas inferiores e expor documentação Swagger.

---

## 💎 Padrões de Design e Boas Práticas

### 1. Result Pattern (`IOperation`)
Para evitar o uso de exceções controladas por fluxo (que degradam a performance do runtime .NET e poluem a arquitetura), a aplicação utiliza o padrão **Result Pattern**. Toda operação expõe uma interface `IOperation`:
- `IOperationSuccess<T>` para cenários bem-sucedidos contendo a carga de dados.
- `IOperationFail` para cenários com erros de negócio, validação ou infraestrutura, permitindo a propagação controlada de erros e mensagens amigáveis.

### 2. Proteção de Invariantes no Domínio
Nossas entidades de domínio (como `BureauEntity`) não expõem construtores públicos que permitam estados inconsistentes ou inválidos. A criação de novos registros é efetuada por meio do método conceitual `ConceptualCreate()`, que valida automaticamente os dados inseridos (como formatação de CEPs, regras e consistência de enums).

### 3. Validação de Caso de Uso Separada
As validações dos payloads de entrada ocorrem de forma apartada na camada de aplicação através de validadores dedicados (baseados na interface `IUseCaseValidation<TInput>`), capturando inconsistências no input antes que a regra de negócio ou os serviços externos sejam acionados.

---

## ⚙️ Configuração e Execução

### Pré-requisitos
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/download/) instalado e rodando (ou rodando via container Docker)

### Configuração do Banco de Dados
Configure a string de conexão no arquivo `appsettings.json` ou `appsettings.Development.json` dentro do projeto `BureauHexagonal.Api`:

```json
"PostgresDbOptions": {
  "Host": "localhost",
  "Port": 5432,
  "DataBase": "BureauHexagonalDb",
  "User": "seu_usuario",
  "Password": "sua_senha"
}
```

### Rodando as Migrações
Com o CLI do EF Core instalado, aplique as migrations para criar a estrutura no PostgreSQL:
```bash
dotnet ef database update --project src/BureauHexagonal.Infrastructure --startup-project src/BureauHexagonal.Api
```

### Executando a Aplicação
Inicie o servidor de desenvolvimento da API:
```bash
dotnet run --project src/BureauHexagonal.Api
```
A documentação do Swagger estará disponível para testes em: `http://localhost:5000/swagger` ou `https://localhost:5001/swagger`.

---

## 🧪 Testes Automatizados

O ecossistema de testes do projeto é de alta confiabilidade e cobertura, escrito com **xUnit**, **Moq** e **FluentAssertions**.

Para fins de máxima clareza e documentação viva, todos os testes são declarados com nomes descritivos em português diretamente integrados na engine de testes:

### Executando os Testes via Terminal
Para executar toda a suíte de testes (Unitários + Integração):
```bash
dotnet test
```

Para rodar apenas os testes unitários:
```bash
dotnet test tests/BureauHexagonal.Test.Unit
```

---

## 📝 Licença

Este projeto é desenvolvido para fins de estudos sobre práticas avançadas de engenharia de software no ecossistema C# (.NET 8). Sinta-se livre para clonar, explorar e estender!
