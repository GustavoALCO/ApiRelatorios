# 📋 Guia Prático: Melhorar o Código em Passos

## 🎯 Visão Geral

Vamos melhorar seu código em **3 Fases**:

```
FASE 1: Renomear (30 min)     → Sem risco ✅
FASE 2: Validação (2h)        → Ganho médio ✅
FASE 3: Dispatcher (4-6h)     → Ganho enorme ✅
```

**Total: ~8h | Ganho: 4.4 → 8.5 score CQRS**

---

## ⏱️ FASE 1: Renomear Queries (30 min)

### Passo 1.1: Identifique as Queries

```bash
# Listar todos os arquivos que precisam renomear
find ./APIRelatorios.Application/Features/Querys -name "*Commands.cs"
```

**Arquivos encontrados:**
```
APIRelatorios.Application/Features/Querys/Rota/BuscarRotaFiltersCommands.cs
APIRelatorios.Application/Features/Querys/EvidenciaRota/BuscarTodasEvidenciasRotaCommands.cs
APIRelatorios.Application/Features/Querys/EvidenciaRota/BuscarEvidenciaPorIDCommands.cs
APIRelatorios.Application/Features/Querys/User/Handler/... (commands em queries)
```

### Passo 1.2: Renomear Arquivos

**Via terminal (Linux/Mac):**
```bash
cd APIRelatorios.Application/Features/Querys/Rota
mv BuscarRotaFiltersCommands.cs BuscarRotaFiltersQuery.cs
```

**Via terminal (Windows PowerShell):**
```powershell
cd APIRelatorios.Application/Features/Querys/Rota
Rename-Item -Path BuscarRotaFiltersCommands.cs -NewName BuscarRotaFiltersQuery.cs
```

**Ou manualmente no Visual Studio:**
- Right-click no arquivo
- "Rename"
- Digitar novo nome

### Passo 1.3: Atualizar Conteúdo dos Arquivos

**Antes:**
```csharp
namespace APIRelatorios.Application.Features.Querys.Rota;

public class BuscarRotaFiltersCommands
{
    public int FiscalId { get; set; }
    public string? Nome { get; set; }
}
```

**Depois:**
```csharp
namespace APIRelatorios.Application.Features.Querys.Rota;

public class BuscarRotaFiltersQuery  // ✅ Renomeado
{
    public int FiscalId { get; set; }
    public string? Nome { get; set; }
}
```

**VS Studio auto-refactor:**
1. Right-click no classe `BuscarRotaFiltersCommands`
2. "Rename" (Ctrl+R, Ctrl+R)
3. Digita `BuscarRotaFiltersQuery`
4. Clica "Apply"
5. **Atualiza todas as referências automaticamente**

### Passo 1.4: Atualizar Referências

**No Controller:**
```csharp
// ❌ ANTES
[HttpGet]
public async Task<IActionResult> BuscarPorFiltro(
    [FromQuery] BuscarRotaFiltersCommands command)
{
    var rota = await _buscarRotaFilters.Handler(command);
    return Ok(rota);
}

// ✅ DEPOIS
[HttpGet]
public async Task<IActionResult> BuscarPorFiltro(
    [FromQuery] BuscarRotaFiltersQuery query)  // ✅ Renomeado
{
    var rota = await _buscarRotaFilters.Handler(query);
    return Ok(rota);
}
```

### Passo 1.5: Verificar Compilação

```bash
dotnet build
```

**Esperado:** ✅ Sem erros

### ✅ Fase 1 Completa!

Arquivos renomeados:
- [ ] `BuscarRotaFiltersCommands.cs` → `BuscarRotaFiltersQuery.cs`
- [ ] `BuscarTodasEvidenciasRotaCommands.cs` → `BuscarTodasEvidenciasRotaQuery.cs`
- [ ] `BuscarEvidenciaPorIDCommands.cs` → `BuscarEvidenciaPorIDQuery.cs`

---

## 🔍 FASE 2: Centralizar Validação (2h)

### Passo 2.1: Criar Pasta de Validadores

```bash
# Criar estrutura
mkdir APIRelatorios.Application/Validators
mkdir APIRelatorios.Application/Validators/Rota
mkdir APIRelatorios.Application/Validators/EvidenciaRota
mkdir APIRelatorios.Application/Validators/User
```

### Passo 2.2: Criar Primeiro Validador

**Arquivo: `APIRelatorios.Application/Validators/Rota/BuscarRotaFiltersValidator.cs`**

```csharp
using APIRelatorios.Application.Features.Querys.Rota;
using FluentValidation;

namespace APIRelatorios.Application.Validators.Rota;

public class BuscarRotaFiltersValidator : AbstractValidator<BuscarRotaFiltersQuery>
{
    public BuscarRotaFiltersValidator()
    {
        RuleFor(x => x.FiscalId)
            .GreaterThan(0)
            .WithMessage("FiscalId deve ser maior que 0");

        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Página deve ser maior que 0");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("PageSize deve estar entre 1 e 100");

        When(x => !string.IsNullOrEmpty(x.DataInicial), () =>
        {
            RuleFor(x => x.DataInicial)
                .Matches(@"^\d{2}/\d{2}/\d{4}$")
                .WithMessage("Data deve estar no formato dd/MM/yyyy");
        });
    }
}
```

### Passo 2.3: Criar Validador para Command

**Arquivo: `APIRelatorios.Application/Validators/Rota/CreateRotaValidator.cs`**

```csharp
using APIRelatorios.Application.Features.Commands.Rota;
using FluentValidation;

namespace APIRelatorios.Application.Validators.Rota;

public class CreateRotaValidator : AbstractValidator<CreateRotaCommand>
{
    public CreateRotaValidator()
    {
        RuleFor(x => x.NomeRota)
            .NotEmpty()
            .WithMessage("Nome da rota é obrigatório")
            .MaximumLength(200)
            .WithMessage("Nome não pode ter mais de 200 caracteres");

        RuleFor(x => x.Alimentador)
            .NotEmpty()
            .WithMessage("Alimentador é obrigatório");

        RuleFor(x => x.Fiscais)
            .NotEmpty()
            .WithMessage("Mínimo 1 fiscal é necessário");

        RuleForEach(x => x.Fiscais)
            .GreaterThan(0)
            .WithMessage("ID do fiscal deve ser válido");
    }
}
```

### Passo 2.4: Registrar Validadores na DI

**Arquivo: `APIRelatorios.IOC/DependencyInjection.cs`**

Encontre o método `DeclareFluentValidate`:

```csharp
// ❌ ANTES
public static IServiceCollection DeclareFluentValidate(this IServiceCollection services)
{
    services.AddValidatorsFromAssembly(Assembly.Load("APIRelatorios.Application"));
    return services;
}

// ✅ DEPOIS (já está correto!)
public static IServiceCollection DeclareFluentValidate(this IServiceCollection services)
{
    services.AddValidatorsFromAssembly(Assembly.Load("APIRelatorios.Application"));
    return services;
}
```

**Verificar se já está ativado em `Program.cs`:**

```csharp
// Em APIRelatorios.WebAPI/Program.cs
var builder = WebApplicationBuilder.CreateBuilder(args);

builder.Services
    .AddInfra(builder.Configuration)
    .DeclareFluentValidate()  // ✅ Está aqui?
    .DeclareInterfaces()
    .DeclareInterfacesServices()
    .DeclareHandlerAplication()
    .Authentication(builder.Configuration)
    .AddSwagger();
```

### Passo 2.5: Aplicar Validação nos Handlers

**Arquivo: `APIRelatorios.Application/Features/Querys/Rota/Handler/BuscarRotaFiltersHandler.cs`**

```csharp
using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Application.Validators.Rota;
using APIRelatorios.Dommain.Interfaces.Rota;
using APIRelatorios.Dommain.Interfaces.User;
using FluentValidation;

namespace APIRelatorios.Application.Features.Querys.Rota.Handler;

public class BuscarRotaFiltersHandler
{
    private readonly IRotaQuery _rotaQuery;
    private readonly IUserQuery _userQuery;
    private readonly IValidator<BuscarRotaFiltersQuery> _validator;  // ✅ NEW

    public BuscarRotaFiltersHandler(
        IRotaQuery rotaQuery, 
        IUserQuery userQuery,
        IValidator<BuscarRotaFiltersQuery> validator)  // ✅ NEW
    {
        _rotaQuery = rotaQuery;
        _userQuery = userQuery;
        _validator = validator;  // ✅ NEW
    }

    public async Task<ICollection<RotaDTO>> Handler(BuscarRotaFiltersQuery query)
    {
        // ✅ Validar
        var validationResult = await _validator.ValidateAsync(query);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new ValidationException(errors);
        }

        // ❌ REMOVER validação manual
        // var page = commands.page <= 0 ? 1 : commands.page;
        // var pagesize = commands.pagesize <= 0 ? 1 : commands.pagesize;

        // Usar os valores validados
        var page = query.Page;
        var pagesize = query.PageSize;

        // Resto do código...
        var fiscal = await _userQuery.BuscarFiscalId(query.FiscalId) 
            ?? throw new Exception("Fiscal Inválido");

        var queryable = _rotaQuery.BuscarQuery();
        queryable = queryable.Where(x => x.Fiscais.Any(f => f.UserId == query.FiscalId));

        // ... resto da implementação
    }
}
```

### Passo 2.6: Refatorar CreateRotaHandler

**Arquivo: `APIRelatorios.Application/Features/Commands/Rota/Handler/CreateRotaHandler.cs`**

```csharp
using APIRelatorios.Application.Validators.Rota;
using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Interfaces.Rota;
using APIRelatorios.Dommain.Interfaces.User;
using FluentValidation;

namespace APIRelatorios.Application.Features.Commands.Rota.Handler;

public class CreateRotaHandler
{
    private readonly IRotaCommands _rotaCommands;
    private readonly IUserQuery _userQuery;
    private readonly IValidator<CreateRotaCommand> _validator;  // ✅ NEW

    public CreateRotaHandler(
        IRotaCommands rotaCommands, 
        IUserQuery userQuery,
        IValidator<CreateRotaCommand> validator)  // ✅ NEW
    {
        _rotaCommands = rotaCommands;
        _userQuery = userQuery;
        _validator = validator;  // ✅ NEW
    }

    public async Task Handle(CreateRotaCommand command)
    {
        // ✅ Validar centralmente
        var validationResult = await _validator.ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new ValidationException(errors);
        }

        // ❌ REMOVER validações que agora estão no validator
        // if (_commands.Fiscais == null || _commands.Fiscais.Count == 0)
        //     throw new Exception("Fiscais não podem ser nulos ou vazio");

        // ✅ Criar rota
        var rota = new Rota(
            command.rotaId ?? Guid.NewGuid(),
            command.NomeRota,
            command.Concessionarias,
            command.Alimentador,
            DateTime.UtcNow);

        // ✅ Adicionar fiscais
        foreach (var userId in command.Fiscais)
        {
            rota.Fiscais.Add(new UsuarioRota { UserId = userId });
        }

        // ✅ Persistir
        await _rotaCommands.CreateRotaAsync(rota);
    }
}
```

### Passo 2.7: Testar

```bash
dotnet build
dotnet test

# Ou via curl
curl -X GET "https://localhost:7000/Rotas?fiscalId=-1" \
  -H "Authorization: Bearer <token>"
# Esperado: Erro de validação
```

### ✅ Fase 2 Completa!

- [ ] Pasta `Validators/` criada
- [ ] `BuscarRotaFiltersValidator.cs` criado
- [ ] `CreateRotaValidator.cs` criado
- [ ] Validação aplicada nos handlers
- [ ] Validação manual removida
- [ ] Código compilado ✅

---

## 🚀 FASE 3: Implementar Dispatcher (4-6h)

### Passo 3.1: Criar Interfaces Base

**Arquivo: `APIRelatorios.Application/Abstractions/ICommandHandler.cs`**

```csharp
namespace APIRelatorios.Application.Abstractions;

public interface ICommandHandler<T> where T : class
{
    Task Handle(T command);
}
```

**Arquivo: `APIRelatorios.Application/Abstractions/IQueryHandler.cs`**

```csharp
namespace APIRelatorios.Application.Abstractions;

public interface IQueryHandler<TQuery, TResponse> 
    where TQuery : class
{
    Task<TResponse> Handle(TQuery query);
}
```

### Passo 3.2: Criar Dispatcher

**Arquivo: `APIRelatorios.Application/Abstractions/CommandDispatcher.cs`**

```csharp
using APIRelatorios.Application.Abstractions;

namespace APIRelatorios.Application.Dispatchers;

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
            throw new InvalidOperationException(
                $"Nenhum handler registrado para comando: {typeof(T).Name}");

        var method = handlerType.GetMethod(nameof(ICommandHandler<object>.Handle));
        var result = method?.Invoke(handler, new object[] { command });

        if (result is Task task)
            await task;
    }
}
```

**Arquivo: `APIRelatorios.Application/Abstractions/QueryDispatcher.cs`**

```csharp
using APIRelatorios.Application.Abstractions;
using System.Reflection;

namespace APIRelatorios.Application.Dispatchers;

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
        var handlerType = typeof(IQueryHandler<,>)
            .MakeGenericType(typeof(TQuery), typeof(TResponse));

        var handler = _serviceProvider.GetService(handlerType);

        if (handler == null)
            throw new InvalidOperationException(
                $"Nenhum handler registrado para query: {typeof(TQuery).Name}");

        var method = handlerType.GetMethod(nameof(IQueryHandler<object, object>.Handle));
        var result = await (Task<TResponse>)method!.Invoke(handler, new object[] { query })!;

        return result;
    }
}
```

### Passo 3.3: Refatorar Handlers para Implementar Interfaces

**Arquivo: `APIRelatorios.Application/Features/Commands/Rota/Handler/CreateRotaHandler.cs`**

```csharp
using APIRelatorios.Application.Abstractions;  // ✅ NEW
using APIRelatorios.Application.Features.Commands.Rota;
using APIRelatorios.Application.Validators.Rota;
using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Interfaces.Rota;
using APIRelatorios.Dommain.Interfaces.User;
using FluentValidation;

namespace APIRelatorios.Application.Features.Commands.Rota.Handler;

public class CreateRotaHandler : ICommandHandler<CreateRotaCommand>  // ✅ NEW
{
    private readonly IRotaCommands _rotaCommands;
    private readonly IUserQuery _userQuery;
    private readonly IValidator<CreateRotaCommand> _validator;

    public CreateRotaHandler(
        IRotaCommands rotaCommands,
        IUserQuery userQuery,
        IValidator<CreateRotaCommand> validator)
    {
        _rotaCommands = rotaCommands;
        _userQuery = userQuery;
        _validator = validator;
    }

    public async Task Handle(CreateRotaCommand command)  // ✅ Implementa interface
    {
        // Implementação (igual ao passo anterior)
    }
}
```

**Arquivo: `APIRelatorios.Application/Features/Querys/Rota/Handler/BuscarRotaFiltersHandler.cs`**

```csharp
using APIRelatorios.Application.Abstractions;  // ✅ NEW
using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Application.Features.Querys.Rota;
using APIRelatorios.Application.Validators.Rota;
using APIRelatorios.Dommain.Interfaces.Rota;
using APIRelatorios.Dommain.Interfaces.User;
using FluentValidation;

namespace APIRelatorios.Application.Features.Querys.Rota.Handler;

public class BuscarRotaFiltersHandler 
    : IQueryHandler<BuscarRotaFiltersQuery, ICollection<RotaDTO>>  // ✅ NEW
{
    private readonly IRotaQuery _rotaQuery;
    private readonly IUserQuery _userQuery;
    private readonly IValidator<BuscarRotaFiltersQuery> _validator;

    public BuscarRotaFiltersHandler(
        IRotaQuery rotaQuery,
        IUserQuery userQuery,
        IValidator<BuscarRotaFiltersQuery> validator)
    {
        _rotaQuery = rotaQuery;
        _userQuery = userQuery;
        _validator = validator;
    }

    public async Task<ICollection<RotaDTO>> Handle(BuscarRotaFiltersQuery query)  // ✅ Implementa interface
    {
        // Implementação (igual ao passo anterior)
    }
}
```

### Passo 3.4: Registrar Handlers na DI

**Arquivo: `APIRelatorios.IOC/DependencyInjection.cs`**

Adicione este método:

```csharp
public static IServiceCollection AddDispatchers(this IServiceCollection services)
{
    // Registrar dispatchers
    services.AddScoped<CommandDispatcher>();
    services.AddScoped<QueryDispatcher>();

    // Auto-descobrir todos os handlers via reflection
    var assembly = Assembly.Load("APIRelatorios.Application");
    
    var handlers = assembly.GetTypes()
        .Where(t => !t.IsInterface && !t.IsAbstract && 
                    t.GetInterfaces().Any(i => 
                        (i.IsGenericType && (
                            i.GetGenericTypeDefinition() == typeof(ICommandHandler<>) ||
                            i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)
                        ))
                    ))
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

Adicione também os `using`:

```csharp
using APIRelatorios.Application.Abstractions;
using APIRelatorios.Application.Dispatchers;
using System.Reflection;
```

Chame em `Program.cs`:

```csharp
// Em APIRelatorios.IOC/DependencyInjection.cs - método AddInfra
public static IServiceCollection AddInfra(
    this IServiceCollection services,
    IConfiguration configuration)
{
    services.AddDatabase(configuration);
    services.AddDispatchers();  // ✅ NEW
    
    return services;
}
```

### Passo 3.5: Refatorar Controllers

**Arquivo: `APIRelatorios.WebAPI/Controllers/RotaController.cs`**

```csharp
using APIRelatorios.Application.Dispatchers;  // ✅ NEW
using APIRelatorios.Application.Features.Commands.Rota;
using APIRelatorios.Application.Features.Querys.Rota;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace APIRelatorios.WebAPI.Controllers;

[ApiController]
[Route("Rotas")]
public class RotaController : ControllerBase
{
    // ❌ REMOVER todas essas injeções:
    // private readonly AddFiscalRotaHandler _addFiscal;
    // private readonly CreateRotaHandler _createRota;
    // ... etc

    // ✅ ADICIONAR apenas estes:
    private readonly CommandDispatcher _commandDispatcher;
    private readonly QueryDispatcher _queryDispatcher;

    public RotaController(
        CommandDispatcher commandDispatcher,
        QueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> BuscarPorFiltro(
        [FromQuery] BuscarRotaFiltersQuery query)
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

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarPorId(Guid id)
    {
        try
        {
            var query = new BuscarRotaIdQuery { RotaId = id };
            var rota = await _queryDispatcher.ExecuteAsync<
                BuscarRotaIdQuery,
                RotaDTO
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

    [Authorize]
    [HttpPost]
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

    [Authorize]
    [HttpDelete("{rotaId}")]
    public async Task<IActionResult> Delete(Guid rotaId)
    {
        try
        {
            await _commandDispatcher.ExecuteAsync(new DeleteRotaCommand { RotaId = rotaId });
            return Ok(new { message = "Rota deletada com sucesso" });
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

### Passo 3.6: Testar Compilação

```bash
dotnet build
```

**Esperado:** ✅ Sem erros

### Passo 3.7: Teste Funcional

```bash
# Rodar aplicação
dotnet run

# Em outro terminal, testar
curl -X GET "https://localhost:7000/Rotas?fiscalId=1" \
  -H "Authorization: Bearer <seu-token>" \
  --insecure
```

### ✅ Fase 3 Completa!

- [ ] Interfaces `ICommandHandler<T>` e `IQueryHandler<T>` criadas
- [ ] Dispatchers criados
- [ ] Handlers refatorados para implementar interfaces
- [ ] DI registrada
- [ ] Controllers refatorados
- [ ] Código compilado ✅
- [ ] Testes passando ✅

---

## 📊 Resultado Final

### Antes (10 injeções)
```csharp
public class RotaController
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

### Depois (2 injeções)
```csharp
public class RotaController
{
    private readonly CommandDispatcher _commandDispatcher;
    private readonly QueryDispatcher _queryDispatcher;
}
```

---

## 🎯 Checklist Final

### ✅ Fase 1: Renomear (30 min)
- [ ] Arquivos renomeados (*Commands → *Query)
- [ ] Conteúdo de classes atualizado
- [ ] Referências atualizadas
- [ ] Compilação OK

### ✅ Fase 2: Validação (2h)
- [ ] Pasta `Validators/` criada
- [ ] Validadores criados para Queries/Commands
- [ ] Validadores injetados nos handlers
- [ ] Validação manual removida dos handlers
- [ ] Compilação OK
- [ ] Testes funcionais OK

### ✅ Fase 3: Dispatcher (4-6h)
- [ ] Interfaces criadas
- [ ] Dispatchers implementados
- [ ] Handlers refatorados (implementam interfaces)
- [ ] DI registrada
- [ ] Controllers refatorados
- [ ] Compilação OK
- [ ] Testes funcionais OK

---

## 🚀 Próximos Passos Opcionais

Após completar as 3 fases, você pode:

1. **Fazer commit com as mudanças**
   ```bash
   git add -A
   git commit -m "refactor: implement CQRS dispatcher pattern and centralize validation"
   git push
   ```

2. **Melhorias futuras:**
   - [ ] Implementar Result Pattern
   - [ ] Adicionar Logging centralizado
   - [ ] Implementar Pagination automática
   - [ ] Adicionar Retry policies

3. **Documentação:**
   - [ ] Atualizar ARCHITECTURE.md
   - [ ] Criar guia para novos handlers

---

## 📞 Dúvidas?

Se tiver problemas em algum passo, aqui estão as causas mais comuns:

**"Erro ao compilar após renomear"**
→ Rode: `dotnet clean && dotnet build`

**"Handler não registrado no dispatcher"**
→ Verifique se implementa `ICommandHandler<T>` ou `IQueryHandler<T>`

**"Validação não funciona"**
→ Confirme que `AddValidatorsFromAssembly()` está chamado em Program.cs

**"Dispatcher retorna null"**
→ Handler pode não estar registrado corretamente na DI

---

Quer ajuda em algum passo específico? 🚀
