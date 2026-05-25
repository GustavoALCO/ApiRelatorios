# 📊 Análise CQRS Refatorada - SEM MediatR

## 🎯 Nova Estratégia

Vou mostrar como melhorar CQRS **usando apenas .NET nativo** (grátis para empresas).

---

## ❌ Problemas que PODEM ser resolvidos sem MediatR

### 1. **Nomenclatura Confusa** 🔴 FÁCIL

**Antes:**
```csharp
// ❌ Confuso
public class BuscarRotaFiltersCommands { }
```

**Depois:**
```csharp
// ✅ Claro
public class BuscarRotaFiltersQuery { }
```

**Tempo:** 30 minutos  
**Custo:** Zero (apenas renomear)

---

### 2. **Validação Dispersa** 🔴 FÁCIL

**Problema Atual:**
```csharp
// Validação no handler (❌ errado)
public class BuscarRotaFiltersHandler
{
    public async Task<ICollection<RotaDTO>> Handler(BuscarRotaFiltersCommands commands)
    {
        var page = commands.page <= 0 ? 1 : commands.page;  // ❌ Aqui
        var pagesize = commands.pagesize <= 0 ? 1 : commands.pagesize;
    }
}
```

**Solução: Validators Centralizados**
```csharp
// 1. Criar validador
public class BuscarRotaFiltersValidator : AbstractValidator<BuscarRotaFiltersQuery>
{
    public BuscarRotaFiltersValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Página deve ser maior que 0");
            
        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("PageSize entre 1 e 100");
    }
}

// 2. Registrar na DI
services.AddValidatorsFromAssembly(Assembly.Load("APIRelatorios.Application"));

// 3. Usar no handler
public async Task<ICollection<RotaDTO>> Handler(BuscarRotaFiltersQuery commands)
{
    var validator = new BuscarRotaFiltersValidator();
    var result = await validator.ValidateAsync(commands);
    
    if (!result.IsValid)
        throw new ValidationException(result.Errors);
        
    // Resto do código sem validação manual
}
```

**Tempo:** 1-2 horas  
**Custo:** Zero (FluentValidation já está instalado)

---

### 3. **Commands/Queries Mutáveis** 🟡 MÉDIO

**Problema:**
```csharp
// ❌ Mutável
public class BuscarRotaFiltersCommands
{
    public int FiscalId { get; set; }      // Setter público!
}
```

**Solução: Readonly Record Struct**
```csharp
// ✅ Imutável e eficiente
public readonly record struct BuscarRotaFiltersQuery(
    int FiscalId,
    string? Nome,
    string? DataInicial,
    string? DataFinal,
    int Page,
    int PageSize
);

// ✅ Ou classe imutável
public sealed class BuscarRotaFiltersQuery
{
    public int FiscalId { get; init; }
    public string? Nome { get; init; }
    public string? DataInicial { get; init; }
    public string? DataFinal { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
}
```

**Tempo:** 1-2 horas  
**Custo:** Zero

---

## ⚠️ Problemas que PRECISAM de solução manual

### 4. **Acoplamento Alto (10+ Handlers)** 🔴 MÉDIO

**Problema:**
```csharp
// ❌ 10 dependências
public class RotaController : ControllerBase
{
    private readonly AddFiscalRotaHandler _addFiscal;
    private readonly CreateRotaHandler _createRota;
    private readonly DeleteRotaHandler _deleteRota;
    private readonly RemoveFiscalRotaHandler _rmvFiscalRota;
    private readonly UpdateNomeRotaHandler _updateNomeRota;
    private readonly CreateRelatorioHandler _createRelatorio;
    private readonly BuscarRotaFiltersHandler _buscarRotaFilters;
    private readonly BuscarRotaIdHandler _buscarRotaIdHandler;
    private readonly CreateEmergencialHandler _createEmergencial;
    private readonly FinalizarRotaHandler _finalizarRota;
}
```

### Solução 1: Dispatcher Factory (Sem MediatR)

```csharp
// 1. Interface base para handlers
public interface IHandler { }

public interface ICommandHandler<T> : IHandler where T : class
{
    Task Handle(T command);
}

public interface IQueryHandler<TQuery, TResponse> : IHandler 
    where TQuery : class
{
    Task<TResponse> Handle(TQuery query);
}

// 2. Dispatcher Service (simples e grátis)
public class CommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task ExecuteAsync<T>(T command) where T : class
    {
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(typeof(T));
        var handler = _serviceProvider.GetService(handlerType);
        
        if (handler == null)
            throw new InvalidOperationException($"Nenhum handler registrado para {typeof(T).Name}");
        
        var method = handlerType.GetMethod(nameof(ICommandHandler<object>.Handle));
        await (Task)method!.Invoke(handler, new object[] { command })!;
    }
}

public class QueryDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public QueryDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> ExecuteAsync<TQuery, TResponse>(TQuery query) 
        where TQuery : class
    {
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(typeof(TQuery), typeof(TResponse));
        var handler = _serviceProvider.GetService(handlerType);
        
        if (handler == null)
            throw new InvalidOperationException($"Nenhum handler registrado para {typeof(TQuery).Name}");
        
        var method = handlerType.GetMethod(nameof(IQueryHandler<object, object>.Handle));
        return await (Task<TResponse>)method!.Invoke(handler, new object[] { query })!;
    }
}

// 3. Refatorar handlers para implementar interfaces
public class CreateRotaHandler : ICommandHandler<CreateRotaCommand>
{
    private readonly IRotaCommands _rotaCommands;

    public CreateRotaHandler(IRotaCommands rotaCommands)
    {
        _rotaCommands = rotaCommands;
    }

    public async Task Handle(CreateRotaCommand command)
    {
        // Implementação
    }
}

public class BuscarRotaFiltersHandler : IQueryHandler<BuscarRotaFiltersQuery, ICollection<RotaDTO>>
{
    private readonly IRotaQuery _rotaQuery;

    public BuscarRotaFiltersHandler(IRotaQuery rotaQuery)
    {
        _rotaQuery = rotaQuery;
    }

    public async Task<ICollection<RotaDTO>> Handle(BuscarRotaFiltersQuery query)
    {
        // Implementação
    }
}

// 4. Registrar na DI
services.AddScoped<CommandDispatcher>();
services.AddScoped<QueryDispatcher>();

// Registrar todos os handlers (automático com reflection)
var handlers = AppDomain.CurrentDomain.GetAssemblies()
    .SelectMany(s => s.GetTypes())
    .Where(p => typeof(IHandler).IsAssignableFrom(p) && !p.IsInterface);

foreach (var handler in handlers)
{
    var interfaces = handler.GetInterfaces();
    foreach (var @interface in interfaces)
    {
        services.AddScoped(@interface, handler);
    }
}

// 5. Controller fica simples
[ApiController]
[Route("Rotas")]
public class RotaController : ControllerBase
{
    private readonly CommandDispatcher _commandDispatcher;  // ✅ Uma injeção!
    private readonly QueryDispatcher _queryDispatcher;      // ✅ Uma injeção!

    public RotaController(CommandDispatcher commandDispatcher, QueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateRotaCommand command)
    {
        try
        {
            await _commandDispatcher.ExecuteAsync(command);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> BuscarPorFiltro([FromQuery] BuscarRotaFiltersQuery query)
    {
        try
        {
            var result = await _queryDispatcher.ExecuteAsync<BuscarRotaFiltersQuery, ICollection<RotaDTO>>(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
```

**Antes:**
```
RotaController → 10 handlers
```

**Depois:**
```
RotaController → CommandDispatcher/QueryDispatcher → Find handlers dinamicamente
```

**Vantagens:**
- ✅ Uma injeção por tipo (Command/Query)
- ✅ Handlers descobertos automaticamente
- ✅ Fácil testar (mock dispatchers)
- ✅ Zero dependência externa
- ✅ Grátis para empresas

**Tempo:** 3-4 horas  
**Custo:** Zero

---

### Solução 2: Service Locator Pattern (Mais Simples)

```csharp
// Se Dispatcher parece complexo, use Service Locator
public interface IHandlerFactory
{
    ICommandHandler<T> GetCommandHandler<T>() where T : class;
    IQueryHandler<TQuery, TResponse> GetQueryHandler<TQuery, TResponse>() 
        where TQuery : class;
}

public class HandlerFactory : IHandlerFactory
{
    private readonly IServiceProvider _provider;

    public HandlerFactory(IServiceProvider provider) => _provider = provider;

    public ICommandHandler<T> GetCommandHandler<T>() where T : class
        => _provider.GetRequiredService<ICommandHandler<T>>();

    public IQueryHandler<TQuery, TResponse> GetQueryHandler<TQuery, TResponse>() 
        where TQuery : class
        => _provider.GetRequiredService<IQueryHandler<TQuery, TResponse>>();
}

// Controller
public class RotaController : ControllerBase
{
    private readonly IHandlerFactory _factory;

    public RotaController(IHandlerFactory factory)
    {
        _factory = factory;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRotaCommand command)
    {
        var handler = _factory.GetCommandHandler<CreateRotaCommand>();
        await handler.Handle(command);
        return Ok();
    }
}
```

**Tempo:** 1-2 horas  
**Custo:** Zero

---

## ✅ Problemas que JÁ estão BONS

```
✅ Separação clara Commands vs Queries
✅ Handlers com responsabilidade única  
✅ DTOs separados de Entities
✅ Repository Pattern implementado
✅ Controllers bem estruturados
```

---

## 📋 Plano de Refatoração (Sem MediatR)

### Fase 1: Quick Wins (1 semana) 🟢
- [ ] Renomear `*Commands` → `*Query` em Queries
- [ ] Centralizar validações FluentValidation
- [ ] Converter Commands/Queries para `record` ou `sealed class` com `init`

**Esforço:** 4-6 horas  
**ROI:** Alto (melhor organização)

---

### Fase 2: Desacoplamento (2 semanas) 🟡
- [ ] Criar `CommandDispatcher` e `QueryDispatcher`
- [ ] Implementar `ICommandHandler<T>` e `IQueryHandler<T>`
- [ ] Refatorar handlers para implementar interfaces
- [ ] Refatorar Controllers para usar dispatchers
- [ ] Remover 10+ injeções dos Controllers

**Esforço:** 6-8 horas  
**ROI:** Muito alto (fácil testar e estender)

---

### Fase 3: Melhorias Futuras (Opcional) 🔵
- [ ] Implementar `Result<T>` pattern
- [ ] Adicionar transações explícitas
- [ ] Pipeline de validação automático
- [ ] Logging centralizado

**Esforço:** 4-6 horas  
**ROI:** Médio

---

## 📊 Comparativo: Antes vs Depois

```
ANTES (Atual)
─────────────
Controller
├─ CreateRotaHandler
├─ BuscarRotaHandler
├─ DeleteRotaHandler
├─ ... 7 mais
└─ Hard to manage


DEPOIS (Fase 1 + 2)
───────────────────
Controller
├─ CommandDispatcher
└─ QueryDispatcher
    ↓ (encontram handlers dinamicamente)
    ├─ ICommandHandler<CreateRotaCommand>
    ├─ ICommandHandler<DeleteRotaCommand>
    ├─ IQueryHandler<BuscarRotaFiltersQuery, ...>
    └─ ... (auto-discovered)

✅ Testável
✅ Extensível
✅ Mantível
✅ Zero dependências
✅ Grátis para empresas
```

---

## 🎯 Recomendação Final

**Implemente:**
1. ✅ **Renomear Queries** (30 min) - Sem risco
2. ✅ **Dispatcher Pattern** (4-6h) - Ganho imediato
3. ✅ **Validadores Centralizados** (2h) - Código limpo

**Não implemente:**
- ❌ MediatR (pago)
- ❌ Novos frameworks
- ❌ Mudanças radicais

**Resultado:**
- ✅ CQRS bem estruturado
- ✅ Sem dependências comerciais
- ✅ Fácil manutenção
- ✅ 0 custos adicionais

---

## 📁 Estrutura Final Recomendada

```
APIRelatorios.Application/
├── Features/
│   ├── Commands/
│   │   ├── Rota/
│   │   │   ├── CreateRotaCommand.cs         ← record
│   │   │   ├── CreateRotaValidator.cs       ← FluentValidation
│   │   │   └── CreateRotaHandler.cs         ← ICommandHandler<T>
│   │   └── ...
│   │
│   ├── Queries/                             ← Renomeado de "Querys"
│   │   ├── Rota/
│   │   │   ├── BuscarRotaFiltersQuery.cs    ← Renomeado
│   │   │   ├── BuscarRotaFiltersValidator.cs
│   │   │   └── BuscarRotaFiltersHandler.cs  ← IQueryHandler<T, R>
│   │   └── ...
│   │
│   ├── DTOs/
│   │   └── RotaDTO.cs
│   │
│   └── Abstractions/
│       ├── ICommandHandler.cs
│       ├── IQueryHandler.cs
│       ├── CommandDispatcher.cs
│       └── QueryDispatcher.cs
│
├── Validators/
│   ├── CreateRotaValidator.cs
│   ├── BuscarRotaFiltersValidator.cs
│   └── ...
│
└── DependencyInjection.cs
    └── RegisterHandlers() [reflection]
```

---

Quer que eu **implemente as fases 1 e 2** no seu projeto? Posso fazer um PR com:

1. ✅ Renomear Queries
2. ✅ Criar Dispatcher Pattern
3. ✅ Refatorar Handlers
4. ✅ Refatorar Controllers
5. ✅ Centralizar Validação

**Tudo grátis, sem MediatR! 🎉**
