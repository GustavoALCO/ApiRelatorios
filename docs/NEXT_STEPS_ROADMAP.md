# 🗺️ Próximos Passos - Roadmap de Implementação

## 📍 Onde Você Está Agora

```
✅ Análise completa feita
✅ Abstrações criadas (ICommandHandler, IQueryHandler)
✅ Documentação pronta
✅ Estrutura avaliada (8.4/10)
↓
🔴 AQUI: Hora de implementar as melhorias
```

---

## 🎯 Roadmap Recomendado (3 Fases)

### 📋 FASE 1: Renomear "Querys" → "Queries" (5-10 min)

**Status:** 🟢 Muito Fácil | Sem Risco

#### Passo 1.1: Renomear Pasta

```bash
cd APIRelatorios.Application/Features
mv Querys Queries
```

#### Passo 1.2: Atualizar Referências no Visual Studio

1. Abra Visual Studio
2. **Ctrl+H** (Find and Replace)
3. Buscar: `Features.Querys`
4. Substituir por: `Features.Queries`
5. Clique **Replace All**

#### Passo 1.3: Compilar

```bash
dotnet build APiRelatorios.slnx
```

**Esperado:** ✅ Build sem erros

---

### 📋 FASE 2: Criar Dispatcher Pattern (2-3 horas)

**Status:** 🟡 Médio | Risco Baixo

#### Passo 2.1: Criar CommandDispatcher

**Arquivo:** `APIRelatorios.Application/Dispatchers/CommandDispatcher.cs`

```csharp
using APIRelatorios.Application.Abstractions.Messaging;
using System.Reflection;

namespace APIRelatorios.Application.Dispatchers;

public class CommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task ExecuteAsync<T>(T command) where T : class, ICommand
    {
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(typeof(T));
        var handler = _serviceProvider.GetService(handlerType);

        if (handler == null)
            throw new InvalidOperationException(
                $"Handler não registrado para comando: {typeof(T).Name}");

        var method = handlerType.GetMethod(nameof(ICommandHandler<T>.Handle));
        var result = method?.Invoke(handler, new object[] { command });

        if (result is Task task)
            await task;
    }
}
```

#### Passo 2.2: Criar QueryDispatcher

**Arquivo:** `APIRelatorios.Application/Dispatchers/QueryDispatcher.cs`

```csharp
using APIRelatorios.Application.Abstractions.Messaging;

namespace APIRelatorios.Application.Dispatchers;

public class QueryDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public QueryDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> ExecuteAsync<TQuery, TResponse>(TQuery query)
        where TQuery : class, IQuery<TResponse>
    {
        var handlerType = typeof(IQueryHandler<,>)
            .MakeGenericType(typeof(TQuery), typeof(TResponse));

        var handler = _serviceProvider.GetService(handlerType);

        if (handler == null)
            throw new InvalidOperationException(
                $"Handler não registrado para query: {typeof(TQuery).Name}");

        var method = handlerType.GetMethod(nameof(IQueryHandler<TQuery, TResponse>.Handle));
        var result = await (Task<TResponse>)method!.Invoke(handler, new object[] { query })!;

        return result;
    }
}
```

#### Passo 2.3: Registrar Dispatchers na DI

**Arquivo:** `APIRelatorios.IOC/DependencyInjection.cs`

Adicione este método:

```csharp
public static IServiceCollection AddDispatchers(this IServiceCollection services)
{
    services.AddScoped<CommandDispatcher>();
    services.AddScoped<QueryDispatcher>();

    // Auto-descobrir handlers
    var assembly = Assembly.Load("APIRelatorios.Application");
    var handlers = assembly.GetTypes()
        .Where(t => !t.IsInterface && !t.IsAbstract &&
                    t.GetInterfaces().Any(i =>
                        i.IsGenericType && (
                            i.GetGenericTypeDefinition() == typeof(ICommandHandler<>) ||
                            i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)
                        )))
        .ToList();

    foreach (var handler in handlers)
    {
        var interfaces = handler.GetInterfaces()
            .Where(i => i.IsGenericType && (
                i.GetGenericTypeDefinition() == typeof(ICommandHandler<>) ||
                i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)
            ));

        foreach (var @interface in interfaces)
        {
            services.AddScoped(@interface, handler);
        }
    }

    return services;
}
```

E chame em `AddInfra`:

```csharp
public static IServiceCollection AddInfra(
    this IServiceCollection services,
    IConfiguration configuration)
{
    services.AddDatabase(configuration);
    services.AddDispatchers();  // ← ADICIONE
    return services;
}
```

#### Passo 2.4: Compilar

```bash
dotnet build APiRelatorios.slnx
```

**Esperado:** ✅ Build sem erros

---

### 📋 FASE 3: Refatorar Handlers (1-2 horas)

**Status:** 🟡 Médio | Risco Baixo

#### Passo 3.1: Refatorar CreateRotaHandler

**Arquivo:** `APIRelatorios.Application/Features/Commands/Rota/Handler/CreateRotaHandler.cs`

**Antes:**
```csharp
public class CreateRotaHandler
{
    public async Task Handle(CreateRotaCommand command) { }
}
```

**Depois:**
```csharp
using APIRelatorios.Application.Abstractions.Messaging;

public class CreateRotaHandler : ICommandHandler<CreateRotaCommand>
{
    // resto do código igual
    
    public async Task Handle(CreateRotaCommand command) { }
}
```

#### Passo 3.2: Refatorar Query Handlers

**Arquivo:** `APIRelatorios.Application/Features/Queries/Rota/Handler/BuscarRotaFiltersHandler.cs`

**Antes:**
```csharp
public class BuscarRotaFiltersHandler
{
    public async Task<ICollection<RotaDTO>> Handler(BuscarRotaFiltersQuery query) { }
}
```

**Depois:**
```csharp
using APIRelatorios.Application.Abstractions.Messaging;

public class BuscarRotaFiltersHandler 
    : IQueryHandler<BuscarRotaFiltersQuery, ICollection<RotaDTO>>
{
    // resto do código igual
    
    public async Task<ICollection<RotaDTO>> Handle(BuscarRotaFiltersQuery query) { }
}
```

**Nota:** Você terá que fazer isso para todos os handlers (Command e Query)

---

### 📋 FASE 4: Refatorar Controllers (1-2 horas)

**Status:** 🟡 Médio | Risco Médio

#### Passo 4.1: Refatorar RotaController

**Antes:**
```csharp
public class RotaController : ControllerBase
{
    private readonly AddFiscalRotaHandler _addFiscal;
    private readonly CreateRotaHandler _createRota;
    private readonly DeleteRotaHandler _deleteRota;
    // ... 7 mais handlers

    public RotaController(
        AddFiscalRotaHandler addFiscal,
        CreateRotaHandler createRota,
        // ... todos os handlers
    ) { }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateRotaCommand command)
    {
        try
        {
            await _createRota.Handler(command);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
```

**Depois:**
```csharp
using APIRelatorios.Application.Dispatchers;

public class RotaController : ControllerBase
{
    private readonly CommandDispatcher _commandDispatcher;
    private readonly QueryDispatcher _queryDispatcher;

    public RotaController(
        CommandDispatcher commandDispatcher,
        QueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post([FromBody] CreateRotaCommand command)
    {
        try
        {
            await _commandDispatcher.ExecuteAsync(command);
            return Ok(new { message = "Rota criada com sucesso" });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { errors = ex.Errors.Select(e => e.ErrorMessage) });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> BuscarPorFiltro([FromQuery] BuscarRotaFiltersQuery query)
    {
        try
        {
            var rota = await _queryDispatcher.ExecuteAsync<
                BuscarRotaFiltersQuery,
                ICollection<RotaDTO>
            >(query);
            return Ok(rota);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { errors = ex.Errors.Select(e => e.ErrorMessage) });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}
```

#### Passo 4.2: Repetir para Todos os Controllers

- `EvidenciaRotaController.cs`
- `LoginController.cs`
- `FiscalController.cs`

---

## 📊 Estimativa Completa

| Fase | Tarefa | Tempo | Risco | Ganho |
|------|--------|-------|-------|-------|
| **1** | Renomear Querys | 5-10 min | 🟢 Nenhum | 🟡 Médio |
| **2** | Dispatcher | 2-3h | 🟡 Baixo | 🟢 Alto |
| **3** | Refatorar Handlers | 1-2h | 🟡 Baixo | 🟡 Médio |
| **4** | Refatorar Controllers | 1-2h | 🟡 Médio | 🟢 Alto |
| **5** | Testar | 1h | 🟢 Baixo | 🟢 Alto |
| **TOTAL** | | **6-8h** | 🟡 Baixo | 🟢 Alto |

---

## 🎯 Prioridade de Implementação

### Comece Por Aqui (Ordem Recomendada):

1. **FASE 1** ← Comece hoje (5 min)
   - Renomear Querys → Queries

2. **FASE 2** ← Próximo dia (2-3h)
   - Criar Dispatchers
   - Registrar na DI

3. **FASE 3** ← Depois (1-2h)
   - Refatorar handlers um por um

4. **FASE 4** ← Finalize (1-2h)
   - Refatorar controllers

5. **FASE 5** ← Validar (1h)
   - Compilar
   - Testar endpoints

---

## ✅ Checklist de Implementação

### Fase 1: Renomear
- [ ] Pasta `Querys` renomeada para `Queries`
- [ ] Referências atualizadas (Find & Replace)
- [ ] Build OK

### Fase 2: Dispatcher
- [ ] `CommandDispatcher.cs` criado
- [ ] `QueryDispatcher.cs` criado
- [ ] `AddDispatchers()` criado em DependencyInjection.cs
- [ ] Registrado em `AddInfra()`
- [ ] Build OK

### Fase 3: Handlers
- [ ] Todos os Command Handlers implementam `ICommandHandler<T>`
- [ ] Todos os Query Handlers implementam `IQueryHandler<T, R>`
- [ ] Build OK

### Fase 4: Controllers
- [ ] `RotaController` refatorado
- [ ] `EvidenciaRotaController` refatorado
- [ ] `LoginController` refatorado
- [ ] `FiscalController` refatorado
- [ ] Build OK

### Fase 5: Testes
- [ ] Compilação sem erros
- [ ] API roda sem crashes
- [ ] Endpoints respondendo
- [ ] Validação funcionando

---

## 🆘 Se Tiver Dúvida em Algum Passo

Cada passo está detalhado em:
- `/docs/STEP_BY_STEP_IMPROVEMENT.md` ← Leia aqui
- `/docs/CQRS_IMPROVEMENT_NO_MEDIATOR.md` ← Referência técnica

---

## 🚀 Próximo Passo Imediato

### Opção A: Você Mesmo Implementa
```
Comece pela FASE 1: Renomear Querys → Queries
(5 minutos, sem risco)
```

### Opção B: Eu Implemente Tudo
```
Posso fazer um PR com:
✅ FASE 1: Renomear
✅ FASE 2: Dispatcher
✅ FASE 3: Handlers refatorados
✅ FASE 4: Controllers refatorados
(Pronto para usar)
```

### Opção C: Eu Ajudo Com Parte
```
Qual fase você quer que eu implemente?
1️⃣ Só Dispatcher?
2️⃣ Dispatcher + Handlers?
3️⃣ Tudo menos Controllers?
```

---

**Qual você prefere? 🎯**

A. Eu começo sozinho (FASE 1 agora!)  
B. Você faz tudo para mim (PR pronto)  
C. Ajuda em uma fase específica
