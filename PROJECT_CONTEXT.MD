# Contexto do Projeto: BureauHexagonal

## 1. Objetivo do sistema
O BureauHexagonal é um sistema de bureau de dados responsável por consultar e normalizar informações provenientes de múltiplos provedores externos (ViaCep, BrasilApi), expondo APIs padronizadas para consumo interno e externo.

## 2. Padrão Arquitetural
O projeto adota os padrões de **Domain-Driven Design (DDD)** e **Arquitetura Hexagonal**, divididos nas seguintes camadas:
- **BureauHexagonal.Api**: Camada de entrada (Controllers, Endpoints, DI, Configurações de Middleware).
- **BureauHexagonal.Core (ou Domain)**: Entidades de negócio, Agregados, Value Objects, Interfaces de Repositório e Regras Próprias que não dependem de frameworks.
- **BureauHexagonal.Infrastructure**: Implementações de acesso a banco de dados, clientes HTTP e serviços externos.

## Regras Arquiteturais
- O Domain/Core NÃO pode depender de Infrastructure.
- Toda comunicação externa deve ocorrer via Ports/Interfaces.
- Casos de uso devem ficar na camada Application.
- Controllers não devem conter regra de negócio.
- Entidades devem proteger invariantes.
- Value Objects devem ser imutáveis.
- Services de domínio só devem existir quando a regra não pertence a uma entidade específica.

## Estrutura de Pastas

/src
  /BureauHexagonal.Api
  /BureauHexagonal.Application
  /BureauHexagonal.Core
  /BureauHexagonal.Infrastructure

/tests
  /BureauHexagonal.UnitTests
  /BureauHexagonal.IntegrationTests

## Convenções

- Commands devem terminar com `Command`
- Queries devem terminar com `Query`
- Handlers devem terminar com `Handler`
- DTOs devem terminar com `Dto`
- Interfaces devem iniciar com `I`
- Eventos devem terminar com `Event`
- Value Objects devem ser `sealed`

## Stack

- .NET 8
- DynamoDB
- Entity Framework 8
- Serilog
- xUnit

## Testes

- Utilizamos xUnit
- Nome do padrão:
  Metodo_Cenario_ResultadoEsperado

Exemplo:
CreateAsync_WhenDocumentAlreadyExists_ShouldThrowConflictException

- Utilizar AAA (Arrange, Act, Assert)
- Utilizar FluentAssertions
- Não mockar entidades de domínio
- Separacao de testes por pastas e funcionalidades

## Tratamento de Erros

## DynamoDB
- Single Table Design
- Evitar Scan
- Priorizar Query
- GSIs devem ser explicitamente documentados
- Entidades devem possuir métodos para composição de chaves

## Observabilidade
- Logs estruturados com correlationId
- Não logar dados sensíveis

## Regras de Código
- Preferir `sealed record` para DTOs imutáveis
- Utilizar `required` quando aplicável
- Evitar métodos estáticos
- Utilizar async/await em operações IO
- Não utilizar regions
- Métodos privados abaixo dos públicos
- Utilizar IOperation (localizado em E:\Estudos\C#\BureauHexagonal\src\BureauHexagonal.Core\Common\Operation\IOperation.cs) como ResultPattern
- Toda requisição/chamada de método deve retornar um IOperation

## Exemplo de Caso de Uso
1. Todo caso de uso deve herdar de IUseCase<TInput> (localizado em E:\Estudos\C#\BureauHexagonal\src\BureauHexagonal.Core\Common\UseCase\IUseCase.cs)
2. Todo caso de uso deve ter uma validacao
3. Toda validacao do caso de uso deve herdar de IUseCaseValidation<TInput>(localizado em E:\Estudos\C#\BureauHexagonal\src\BureauHexagonal.Core\Common\UseCase\IUseCaseValidation.cs) sendo q TInput é do mesmo tipo do caso de uso
4. Todo caso de uso deve ter um try catch para tratar erros, somente nele que fica o try catch, com excecao de repositórios.

Fluxo: 
1. Chamada da validacao do caso de uso
2. Verificacao se ocorreu erro
3. Chamada de serviços

## Diretrizes para IA
Ao gerar código:
- Sempre respeitar DDD, Arquitetura Hexagonal e SOLID
- Nunca acessar Infrastructure diretamente do Domain
- Sempre gerar testes unitários
- Sempre considerar concorrência e idempotência
- Priorizar baixo acoplamento
- Seguir SOLID a risca