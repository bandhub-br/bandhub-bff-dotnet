# 🎸 BandHub — BFF (Backend for Frontend)

<p align="center">
  <strong>Gateway de entrada do BandHub — orquestra chamadas aos microsserviços</strong>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 8" />
  <img src="https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black" alt="Swagger" />
  <img src="https://img.shields.io/badge/xUnit-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt="xUnit" />
</p>

---

## 📋 Índice

- [Sobre o Serviço](#-sobre-o-serviço)
- [Arquitetura](#-arquitetura)
- [Tecnologias](#-tecnologias)
- [Estrutura do Projeto](#-estrutura-do-projeto)
- [Pré-requisitos](#-pré-requisitos)
- [Configuração e Instalação](#-configuração-e-instalação)
- [Executando a Aplicação](#-executando-a-aplicação)
- [Testes](#-testes)
- [Endpoints da API](#-endpoints-da-api)
- [Contribuindo](#-contribuindo)

---

## 📖 Sobre o Serviço

O **BFF (Backend for Frontend)** é o ponto de entrada do BandHub. Ele atua como um gateway que recebe requisições dos clientes e as orquestra entre os microsserviços internos (**UserService** e **BandService**), expondo uma API unificada e simplificada para o frontend.

| Porta | Descrição |
|-------|-----------|
| `5223` | Gateway de entrada — roteamento e orquestração de chamadas |

### Microsserviços integrados

| Serviço | URL interna |
|---------|-------------|
| **UserService** | `http://localhost:5293` |
| **BandService** | `http://localhost:5081` |

---

## 🏗 Arquitetura

O projeto segue a arquitetura **Vertical Slice Architecture**, onde cada feature é organizada em sua própria pasta. As integrações com os microsserviços downstream são isoladas na pasta `Integrations/`.

```
Feature/
├── Endpoint.cs      → Define a rota HTTP (Minimal API)
├── Handler.cs       → Orquestra chamadas aos microsserviços
└── Request.cs       → Contrato de entrada

Integrations/
├── UserService/
│   ├── UserServiceClient.cs         → Cliente HTTP para o UserService
│   ├── LoginRequest.cs / LoginResponse.cs
│   ├── RegisterAccountRequest.cs / RegisterAccountResponse.cs
└── BandService/
    ├── BandServiceClient.cs         → Cliente HTTP para o BandService
    ├── CreateBandRequest.cs / CreateBandResponse.cs
```

### Princípios aplicados

- **Vertical Slice Architecture** — cada feature isolada com seus próprios componentes
- **BFF Pattern** — API adaptada para as necessidades do cliente frontend
- **Minimal APIs** — endpoints leves e performáticos
- **Dependency Injection** — inversão de dependência nativa do .NET
- **HttpClient** — comunicação HTTP tipada com os microsserviços

---

## 🛠 Tecnologias

| Tecnologia | Versão | Uso |
|------------|--------|-----|
| .NET | 8.0 | Framework principal |
| ASP.NET Core | 8.0 | Web API com Minimal APIs |
| HttpClient | — | Comunicação com microsserviços internos |
| Swagger / Swashbuckle | 6.6.2 | Documentação da API |
| xUnit | 2.5.3 | Framework de testes |
| Moq | 4.20.72 | Mocking para testes unitários |
| FluentAssertions | 8.8.0 | Assertions expressivas |

---

## 📁 Estrutura do Projeto

```
BandHub.Bff/
│
├── BandHub.Bff/                            # Projeto principal
│   ├── Features/
│   │   └── Accounts/
│   │       ├── Login/
│   │       │   ├── LoginEndpoint.cs
│   │       │   ├── LoginHandler.cs
│   │       │   └── LoginRequest.cs
│   │       ├── RegisterUser/
│   │       │   ├── RegisterUserEndpoint.cs
│   │       │   ├── RegisterUserHandler.cs
│   │       │   └── RegisterUserRequest.cs
│   │       └── RegisterBand/
│   │           ├── RegisterBandEndpoint.cs
│   │           ├── RegisterBandHandler.cs
│   │           ├── RegisterBandRequest.cs
│   │           └── RegisterBandResponse.cs
│   ├── Integrations/
│   │   ├── UserService/
│   │   │   ├── UserServiceClient.cs
│   │   │   ├── LoginRequest.cs
│   │   │   ├── LoginResponse.cs
│   │   │   ├── RegisterAccountRequest.cs
│   │   │   └── RegisterAccountResponse.cs
│   │   └── BandService/
│   │       ├── BandServiceClient.cs
│   │       ├── CreateBandRequest.cs
│   │       └── CreateBandResponse.cs
│   ├── Common/
│   ├── Program.cs
│   ├── appsettings.json
│   └── BandHub.Bff.csproj
│
├── tests/
│   └── BandHub.Bff.UnitTests/
│       └── Features/
│           └── Accounts/
│               └── Login/
│                   └── LoginHandlerTests.cs
│
├── BandHub.Bff.sln
└── README.md
```

---

## ✅ Pré-requisitos

Antes de começar, certifique-se de ter instalado:

- [**.NET 8 SDK**](https://dotnet.microsoft.com/download/dotnet/8.0)

> ⚠️ O BFF **não possui banco de dados próprio**. Ele depende dos microsserviços **UserService** e **BandService** estarem em execução.

---

## ⚙ Configuração e Instalação

### 1. Clone o repositório

```bash
git clone https://github.com/bandhub-br/bandhub-bff.git
cd bandhub-bff
```

### 2. Restaure as dependências

```bash
dotnet restore
```

### 3. Certifique-se de que os microsserviços estão rodando

O BFF depende dos seguintes serviços em execução:

| Serviço | Porta |
|---------|-------|
| UserService | `5293` |
| BandService | `5081` |

### 4. Verifique a configuração dos serviços

As URLs dos microsserviços estão no arquivo `appsettings.json`:

**`BandHub.Bff/appsettings.json`**
```json
{
  "Services": {
    "UserServiceBaseUrl": "http://localhost:5293",
    "BandServiceBaseUrl": "http://localhost:5081"
  }
}
```

---

## 🚀 Executando a Aplicação

```bash
# BFF (porta 5223)
dotnet run --project BandHub.Bff
```

### Acessar o Swagger

Após iniciar o serviço, acesse a documentação interativa:

| URL |
|-----|
| http://localhost:5223/swagger |

### Build do projeto

```bash
dotnet build
```

---

## 🧪 Testes

O projeto utiliza **xUnit** como framework de testes, **Moq** para mocking e **FluentAssertions** para assertions expressivas.

### Executar todos os testes

```bash
dotnet test
```

### Executar testes do projeto de testes

```bash
dotnet test tests/BandHub.Bff.UnitTests
```

### Executar com output detalhado

```bash
dotnet test --verbosity detailed
```

### Executar com cobertura de código

```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Estrutura dos testes

```
tests/
└── BandHub.Bff.UnitTests/
    └── Features/Accounts/
        └── Login/
            └── LoginHandlerTests.cs   → Testa orquestração do login
```

### Padrão dos testes

Todos os testes seguem o padrão **AAA (Arrange-Act-Assert)**:

```csharp
[Fact]
public async Task HandleAsync_ShouldReturnLoginResponse_WhenCredentialsAreValid()
{
    // Arrange - preparar mocks do UserServiceClient
    _userServiceClientMock
        .Setup(c => c.LoginAsync(It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(new LoginResponse(...));

    // Act - executar a ação
    var response = await _handler.HandleAsync(request, CancellationToken.None);

    // Assert - verificar o resultado
    response.Should().NotBeNull();
}
```

---

## 📡 Endpoints da API

| Método | Rota | Descrição | Microsserviço |
|--------|------|-----------|---------------|
| `POST` | `/accounts/login` | Autenticar uma conta | UserService |
| `POST` | `/accounts/register/user` | Registrar uma conta de usuário | UserService |
| `POST` | `/accounts/register/band` | Registrar uma conta de banda | UserService + BandService |

#### `POST /accounts/login`

**Request:**
```json
{
  "email": "john@example.com",
  "password": "password123"
}
```

**Response (200):**
```json
{
  "accountId": "550e8400-e29b-41d4-a716-446655440000",
  "name": "John Doe",
  "email": "john@example.com",
  "accountType": "User"
}
```

#### `POST /accounts/register/user`

**Request:**
```json
{
  "name": "John Doe",
  "email": "john@example.com",
  "password": "password123"
}
```

**Response (201):**
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "name": "John Doe",
  "email": "john@example.com",
  "accountType": "User",
  "createdAt": "2026-03-07T15:30:00Z"
}
```

#### `POST /accounts/register/band`

Registra uma conta do tipo Band no **UserService** e em seguida cria o perfil da banda no **BandService**.

**Request:**
```json
{
  "name": "Arctic Monkeys",
  "email": "arctic@example.com",
  "password": "password123",
  "bandName": "Arctic Monkeys",
  "description": "Banda inglesa de indie rock",
  "genre": "Indie Rock",
  "spotifyId": "7Ln80lUS6He07XvHI8qqHH"
}
```

**Response (201):**
```json
{
  "id": "660e8400-e29b-41d4-a716-446655440000",
  "accountId": "550e8400-e29b-41d4-a716-446655440000",
  "name": "Arctic Monkeys",
  "genre": "Indie Rock",
  "description": "Banda inglesa de indie rock",
  "spotifyId": "7Ln80lUS6He07XvHI8qqHH",
  "createdAt": "2026-03-07T15:30:00Z"
}
```

---

## 🤝 Contribuindo

1. Crie uma branch a partir da `main`:
   ```bash
   git checkout -b feature/minha-feature
   ```

2. Faça suas alterações seguindo a **Vertical Slice Architecture**

3. Escreva testes unitários para sua feature

4. Execute os testes e garanta que todos passam:
   ```bash
   dotnet test
   ```

5. Faça o commit e abra um Pull Request

---

<p align="center">
  Feito com ❤️ pelo time <strong>BandHub</strong>
</p>
