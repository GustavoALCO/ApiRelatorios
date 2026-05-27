# 🏛️ Arquitetura - API Relatórios

## Padrão de Arquitetura

Este projeto segue a **Clean Architecture** com separação clara de responsabilidades:

```
┌─────────────────────────────────────────────────────────────┐
│                    APRESENTAÇÃO (WebAPI)                     │
│        Controllers HTTP → Swagger → Client (Flutter)         │
└─────────────────┬───────────────────────────────────────────┘
                  │ DTOs (Commands/Queries)
                  ▼
┌─────────────────────────────────────────────────────────────┐
│                 APLICAÇÃO (Application)                      │
│    Handlers (CQRS) → Serviços → Validações                   │
│    FluentValidation → Business Logic                         │
└─────────────────┬───────────────────────────────────────────┘
                  │ Domain Entities
                  ▼
┌─────────────────────────────────────────────────────────────┐
│                   DOMÍNIO (Domain)                           │
│     Entities → Value Objects → Interfaces → Enums            │
│     Regras de negócio core                                   │
└─────────────────┬───────────────────────────────────────────┘
                  │ Repository Pattern
                  ▼
┌─────────────────────────────────────────────────────────────┐
│              INFRAESTRUTURA (Infrastructure)                 │
│    EF Core → PostgreSQL → Azure Services → Storage           │
│    Persistência → Autenticação → Integrações                 │
└─────────────────────────────────────────────────────────────┘
```

---

## 📂 Camadas Detalhadas

### 1. **Presentation (APIRelatorios.WebAPI)**

**Responsabilidade:** Receber requisições HTTP e retornar respostas

**Componentes:**
- `Controllers/` - Endpoints HTTP
  - `RotaController` - Gerenciar rotas
  - `EvidenciaRotaController` - Gerenciar evidências
  - `LoginController` - Autenticação
  - `FiscalController` - Gerenciar usuários/fiscais
- `Program.cs` - Configuração da aplicação
- Middleware - CORS, Autenticação, Logging

**Exemplo:**
```csharp
[ApiController]
[Route("EvidenciaRotas")]
public class EvidenciaRotaController : ControllerBase
{
    private readonly CreateEvidenciaHandler _handler;
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CriarEvidencias(CreateEvidenciaCommand command)
    {
        await _handler.Handler(command);
        return Ok();
    }
}
```

---

### 2. **Application (APIRelatorios.Application)**

**Responsabilidade:** Orquestrar a lógica de negócio

**Padrão:** CQRS (Command Query Responsibility Segregation)

**Estrutura:**
```
Features/
├── Commands/          # Operações que modificam estado
│   ├── Rota/
│   │   ├── CreateRotaCommand (DTO)
│   │   └── Handler/CreateRotaHandler.cs
│   ├── EvidenciaRota/
│   │   ├── CreateEvidenciaCommand
│   │   └── Handler/CreateEvidenciaHandler.cs
│   └── User/
│       ├── CreateUserCommand
│       └── Handler/CreateUserHandler.cs
│
├── Queries/           # Operações de leitura
│   ├── Rota/
│   │   ├── BuscarRotaFiltersCommands (DTO)
│   │   └── Handler/BuscarRotaFiltersHandler.cs
│   └── EvidenciaRota/
│       ├── BuscarTodasEvidenciasRotaCommands
│       └── Handler/BuscarTodasAsEvidenciasRotaHandler.cs
│
├── Interfaces/        # Contratos de serviços
│   ├── ISavedImages
│   ├── IRelatorioDeIrregularidades
│   └── etc
│
└── Services/          # Implementação de serviços
    ├── SavedImage.cs
    ├── RelatorioDeIrregulariedades.cs
    ├── BuscarByteImagemService.cs
    └── etc
```

**Handlers:**

Cada Handler implementa a **lógica de caso de uso**:

```csharp
public class CreateEvidenciaHandler
{
    // Injeções de dependência
    private readonly IEvidenciaRotaCommands _commands;
    private readonly ISavedImages _uploadImage;
    private readonly IRotaQuery _rotaQuery;
    private readonly IUserQuery _userQuery;
    private readonly IAzureMapsEnderecoService _mapsService;
    
    // Orquestra o fluxo
    public async Task Handler(CreateEvidenciaCommand createImage)
    {
        // 1. Valida
        var rota = await _rotaQuery.BuscarRotaID(createImage.rotaID);
        var fiscal = await _userQuery.BuscarFiscalId(createImage.fiscalId);
        
        // 2. Processa
        var urls = await _uploadImage.UploadListBase64ImagesAsync(...);
        var checkList = new CheckList(...);
        var evidencia = new EvidenciaRota(...);
        
        // 3. Persiste
        await _commands.SaveImage(evidencia);
    }
}
```

---

### 3. **Domain (APIRelatorios.Dommain)**

**Responsabilidade:** Regras de negócio core (language-agnostic)

**Componentes:**

#### Entities (Modelos de Domínio)
```
├── Rota.cs
│   ├─ Identificação única da rota
│   ├─ Relacionamento com EvidenciaRota (1:N)
│   ├─ Relacionamento com UsuarioRota (1:N)
│   └─ Métodos: AdicionarFiscal(), AlterarNomeRota(), FinalizandoRota()
│
├── EvidenciaRota.cs
│   ├─ Evidência de uma inspeção
│   ├─ Relacionamento com CheckList (1:1)
│   ├─ Relacionamento com ImageData (1:N)
│   └─ Métodos: Atualizar(), DesativarEvidencia()
│
├── CheckList.cs
│   ├─ Tema e subtemas da fiscalização
│   └─ Enums: TemaCheck, SubTemaAlimentadores
│
├── User.cs
│   ├─ Fiscal do sistema
│   └─ Métodos: CreateUser(), AlterarSenha()
│
└── ImageData.cs
    └─ Referência e metadata da imagem
```

#### Interfaces (Contratos)
```
Interfaces/
├── Rota/
│   ├─ IRotaCommands (CREATE, UPDATE, DELETE)
│   └─ IRotaQuery (READ)
├── EvidenciaRota/
│   ├─ IEvidenciaRotaCommands
│   └─ IEvidenciaRotaQuery
├── Services/
│   ├─ IJwtTokenService
│   ├─ IAzureMapsKmService
│   ├─ IAzureMapsEnderecoService
│   └─ ISavedImages
└── etc
```

#### Enums
```csharp
public enum Concessionarias { AES, Enel, Copel, etc }
public enum TemaCheck { Seguranca, Qualidade, etc }
public enum SubTemaAlimentadores { Via, Transformador, etc }
```

---

### 4. **Infrastructure (APIRelatorios.Infra)**

**Responsabilidade:** Implementação técnica de persistência e integrações

**Componentes:**

#### Database
```csharp
public class DatabaseContext : DbContext
{
    public DbSet<Rota> Rotas { get; set; }
    public DbSet<EvidenciaRota> EvidenciaRota { get; set; }
    public DbSet<User> Fiscais { get; set; }
    public DbSet<UsuarioRota> UsuarioRotas { get; set; }
    
    // EF Core configurations (FluentAPI)
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Relacionamentos, índices, constraints
    }
}
```

#### Repository Pattern
```
Repository/
├── Rota/
│   ├─ RotaCommands.cs     (implements IRotaCommands)
│   └─ RotaQuery.cs        (implements IRotaQuery)
├── EvidenciaRota/
│   ├─ EvidenciaRotaCommands.cs
│   └─ EvidenciaRotaQuery.cs
└── User/
    └─ etc
```

**Exemplo:**
```csharp
public class EvidenciaRotaCommands : IEvidenciaRotaCommands
{
    private readonly DatabaseContext _context;
    
    public async Task SaveImage(EvidenciaRota img)
    {
        await _context.EvidenciaRota.AddAsync(img);
        await _context.SaveChangesAsync();
    }
}
```

#### Autenticação
```
Auth/
├── JWTTokenService.cs     # Geração e validação de tokens
└── PasswordHasher.cs      # Hash de senhas
```

#### Serviços de Integração
```
├── AzureMapsKmService.cs         # Cálculo de km via Azure Maps
├── AzureMapsEnderecoService.cs   # Reverse geocoding
└── SavedImage.cs                 # Upload para Azure Blob
```

---

### 5. **Cross-Cutting (APIRelatorios.IOC)**

**Responsabilidade:** Dependency Injection e configurações globais

```csharp
public static class DependencyInjection
{
    // Registra todos os serviços
    public static IServiceCollection AddInfra(...)
    {
        services.AddDatabase(configuration);
        services.AddSwagger();
        services.DeclareInterfaces();
        services.DeclareInterfacesServices();
        services.DeclareHandlerAplication();
        services.DeclareFluentValidate();
        services.Authentication(configuration);
    }
    
    // Seed de dados iniciais
    public static async Task SeedAsync(IServiceProvider services)
    {
        // Cria usuário admin padrão
    }
}
```

**Configurações:**
- Database Context
- Swagger
- JWT Bearer
- FluentValidation
- Services do Azure

---

## 🔄 Fluxo de Requisição Típico

```
1. Client (Flutter) 
   ↓ POST /api/evidencias com JWT
   
2. EvidenciaRotaController
   ├─ Valida Authorization (JWT)
   ├─ Desserializa CreateEvidenciaCommand
   ├─ Chama CreateEvidenciaHandler
   
3. CreateEvidenciaHandler
   ├─ Valida Rota (via IRotaQuery)
   ├─ Valida Fiscal (via IUserQuery)
   ├─ Upload de imagens (via ISavedImages)
   ├─ Busca endereço (via IAzureMapsEnderecoService)
   ├─ Cria entidades (Domain)
   
4. EvidenciaRotaCommands (Repository)
   ├─ DbContext.AddAsync(evidencia)
   ├─ SaveChangesAsync()
   
5. PostgreSQL
   ├─ INSERT INTO EvidenciaRota
   ├─ INSERT INTO CheckList
   ├─ INSERT INTO ImageData
   
6. Resposta
   ├─ 200 OK
   └─ Retorna para Flutter
```

---

## 🧩 Padrões de Design Utilizados

| Padrão | Onde | Propósito |
|--------|------|----------|
| **Repository** | Infra | Abstração de persistência |
| **CQRS** | Application | Separar leitura de escrita |
| **Dependency Injection** | IOC | Injeção de dependências |
| **Fluent Validation** | Application | Validações estruturadas |
| **Entity Framework** | Infra | ORM e migrations |
| **JWT Bearer** | Auth | Autenticação stateless |
| **Value Objects** | Domain | Objetos com identidade |

---

## 📊 Relacionamentos de Entidades

```
┌─────────────────────┐
│      Rota           │
├─────────────────────┤
│ rotaId (PK)         │
│ nomeRota            │
│ alimentador         │
│ km                  │
│ dataInicio          │
│ dataFinal           │
└───────────┬─────────┘
            │ 1:N
            │
    ┌───────────────────────────────────┐
    │                                   │
    ▼                                   ▼
┌─────────────────────┐      ┌─────────────────────┐
│  EvidenciaRota      │      │   UsuarioRota       │
├─────────────────────┤      ├─────────────────────┤
│ evidenciaId (PK)    │      │ usuarioRotaId (PK)  │
│ rotaId (FK)         │      │ rotaId (FK)         │
│ fiscalId (FK)       │      │ userId (FK)         │
│ checklistId (FK)    │      └─────────────────────┘
│ latitude            │              ▲
│ longitude           │              │ 1:N
│ horario             │              │
└────────┬────────────┘      ┌───────────────────┐
         │ 1:N               │       User        │
         │                   ├───────────────────┤
         ▼                   │ userId (PK)       │
┌─────────────────────┐      │ login             │
│    ImageData        │      │ password_hash     │
├─────────────────────┤      │ salt              │
│ imageId (PK)        │      │ isAdmin           │
│ evidenciaId (FK)    │      └───────────────────┘
│ imagemUrl           │
└─────────────────────┘
         ▲
         │ 1:N
         │
┌─────────────────────┐
│    CheckList        │
├─────────────────────┤
│ checklistId (PK)    │
│ tema                │
│ subTemas            │
└─────────────────────┘
```

---

## 🔒 Segurança

### Autenticação
- **JWT Bearer** com RS256 signing
- Tokens com expiração configurável
- Refresh token (opcional)

### Autorização
- `[Authorize]` em todos os endpoints
- Claims baseados em roles (Admin, User)
- Validação de propriedade (usuário só acessa seus dados)

### Validação
- FluentValidation em todos os Commands
- EF Core constraints (NOT NULL, UNIQUE, etc)
- Sanitização de inputs

---

## 🚀 Escalabilidade Futura

**Melhorias possíveis:**
- [ ] Caching com Redis
- [ ] Message Queue (RabbitMQ) para processamentos async
- [ ] Pagination automática
- [ ] Logging estruturado com Serilog
- [ ] Rate limiting
- [ ] API versioning
- [ ] GraphQL (alternativa a REST)

---

**Próxima leitura:** [SETUP.md](./SETUP.md)
