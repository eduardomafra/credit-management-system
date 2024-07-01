# credit-management-system

Este tutorial fornece os passos para executar os serviços usando Docker ou diretamente usando o .NET CLI.

#### Pré-requisitos

- .NET SDK 8.0
- Docker (caso execute com Docker)
- Instância do RabbitMQ (caso execute sem docker)

## Executando sem Docker

#### 1. Alerta antes de iniciar o processo

Antes de iniciar o processo de execução de migrations e compilação dos projetos, certifique-se de que o RabbitMQ e o SQL Server estejam rodando localmente e devidamente configurados no appsettings.json de cada serviço. Caso contrário, pode ocorrer um erro.

#### 2. Aplicando Migrations

Abra um terminal no diretório raiz do projeto e execute as migrations para criação de tabelas:

```bash
  dotnet ef database update --startup-project customer-service\src\CustomerService.API\ --project customer-service\src\CustomerService.Infrastructure\
```
```bash
  dotnet ef database update --startup-project credit-proposal-service\src\CreditProposalService.Api\ --project credit-proposal-service\src\CreditProposalService.Infrastructure\
```
```bash
  dotnet ef database update --startup-project credit-card-service\src\CreditCardService.Api\ --project credit-card-service\src\CreditCardService.Infrastructure\
```

#### 3. Restaurando dependências

No diretório raiz do projeto e restaure as dependências:
```bash
  dotnet restore customer-service\src\CustomerService.API\
```
```bash
  dotnet restore credit-proposal-service\src\CreditProposalService.Api\
```
```bash
  dotnet restore credit-card-service\src\CreditCardService.Api
```

#### 4. Compilando os projetos

No diretório raiz do projeto, compile os projetos:

```bash
  dotnet build customer-service\src\CustomerService.API\
```
```bash
  dotnet build credit-proposal-service\src\CreditProposalService.Api\
```
```bash
  dotnet build credit-card-service\src\CreditCardService.Api
```

#### 5. Executando os serviços

Na pasta raiz do projeto, execute os serviços:

```bash
  dotnet run --project customer-service\src\CustomerService.API\
```
```bash
  dotnet run --project credit-proposal-service\src\CreditProposalService.Api\
```
```bash
  dotnet run --project credit-card-service\src\CreditCardService.Api
```

#### 6. Verificando a execução

Acesse o swagger do serviço de clientes com a URL: http://localhost:5001/swagger/index.html

## Executando com Docker

#### 1. Construindo imagens com Docker Compose

Abra um terminal no diretório raiz do projeto e execute:

```bash
  docker-compose up --build
```
#### 2. Verificando a execução

Acesse o swagger do serviço de clientes com a URL: http://localhost:5001/swagger/index.html

## Exemplo de consumo de endpoint

#### Rota

```http
  POST /api/customer
```
#### Exemplo de body

```json
{
  "name": "João Silva",
  "document": "77561366086",
  "birthDate": "1990-01-01T00:00:00Z",
  "email": "joao.silva@example.com",
  "phone": "11912345678",
  "financialProfile": {
    "monthlyIncome": 5000,
    "creditScore": 700,
    "ownsHome": true,
    "ownsVehicle": true
  }
}
```

#### CURL

```curl
curl -X 'POST' \
  'http://localhost:5001/api/Customer' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "name": "João Silva",
  "document": "77561366086",
  "birthDate": "1990-01-01T00:00:00Z",
  "email": "joao.silva@example.com",
  "phone": "11912345678",
  "financialProfile": {
    "monthlyIncome": 5000,
    "creditScore": 700,
    "ownsHome": true,
    "ownsVehicle": true
  }
}
'
```




