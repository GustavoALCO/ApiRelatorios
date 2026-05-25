# 📚 Documentação - API Relatórios

Bem-vindo à documentação da **API Relatórios**! Este projeto é um sistema completo para gerenciamento de rotas de inspeção, evidências fotográficas e geração de relatórios.

## 📑 Índice

- **[1. Visão Geral](./README.md)** ← Você está aqui
- **[2. Arquitetura](./ARCHITECTURE.md)** - Estrutura e padrões
- **[3. Configuração](./SETUP.md)** - Como rodar o projeto
- **[4. API Reference](./API.md)** - Endpoints e exemplos
- **[5. Fluxos de Negócio](./FLOWS.md)** - Casos de uso principais
- **[6. Banco de Dados](./DATABASE.md)** - Schema e relacionamentos

---

## 🎯 O que é este projeto?

A **API Relatórios** é um backend para um sistema de **gestão de rotas de inspeção** com coleta de evidências fotográficas.

**Funcionalidades principais:**

✅ Autenticação JWT  
✅ Gerenciamento de rotas e fiscais  
✅ Upload e armazenamento de imagens (Azure Blob)  
✅ Geolocalização com Azure Maps  
✅ Geração de relatórios em Word (.docx)  
✅ Checklist de fiscalização com temas e subtemas  
✅ Casos emergenciais  

---

## 🛠️ Tech Stack

### Backend
| Tecnologia | Versão | Uso |
|-----------|--------|-----|
| **.NET** | 10 | Framework principal |
| **C#** | 12 | Linguagem |
| **PostgreSQL** | 15+ | Banco de dados |
| **Entity Framework Core** | 8 | ORM |
| **JWT Bearer** | - | Autenticação |
| **Swagger/OpenAPI** | - | Documentação de API |
| **FluentValidation** | - | Validações |
| **Azure Blob Storage** | - | Armazenamento de imagens |
| **Azure Maps** | - | Geolocalização |

### Frontend Relacionado
| Tecnologia | Versão | Uso |
|-----------|--------|-----|
| **Flutter** | 3+ | App mobile |
| **Dart** | - | Linguagem |
| **SQLite** | - | Cache local |

---

## 👥 Públicos

### Desenvolvedores
- Implementar novos endpoints
- Adicionar validações
- Integrar novos serviços
- Debugar e corrigir bugs
- → Veja [ARCHITECTURE.md](./ARCHITECTURE.md)

### Product/Stakeholders
- Entender fluxos de negócio
- Acompanhar funcionalidades
- Validar requisitos
- → Veja [FLOWS.md](./FLOWS.md)

### API Consumers
- Integrar a API
- Fazer requisições HTTP
- Entender payloads
- → Veja [API.md](./API.md)

---

## 🚀 Quick Start

**Pré-requisitos:**
- Docker + Docker Compose
- .NET 10 SDK (opcional, Docker já inclui)
- PostgreSQL 15 (ou via Docker)

**1. Clone e configure:**
```bash
git clone <repo>
cd ApiRelatorios
```

**2. Configure o `appsettings.json`:**
```bash
cp appsettings.example.json appsettings.json
# Edite com suas credenciais
```

**3. Inicie com Docker:**
```bash
docker compose up --build
```

**4. Acesse:**
```
API: https://localhost:7000
Swagger: https://localhost:7000/swagger
```

> Para instruções detalhadas, veja [SETUP.md](./SETUP.md)

---

## 📦 Estrutura do Projeto

```
ApiRelatorios/
├── APIRelatorios.Domain/          # Entities e interfaces
│   ├── Entities/                  # Rota, EvidenciaRota, User, etc
│   ├── Interfaces/                # Contratos
│   └── Enums/                     # TemaCheck, Concessionarias, etc
│
├── APIRelatorios.Application/     # Lógica de negócio
│   ├── Features/
│   │   ├── Commands/              # Handlers de criação/atualização
│   │   └── Queries/               # Handlers de busca
│   ├── Interfaces/                # Contratos de serviços
│   └── Services/                  # Serviços (JWT, Upload, etc)
│
├── APIRelatorios.Infra/           # Persistência
│   ├── Database/                  # EF Core DbContext
│   ├── Repository/                # Implementação de repositories
│   ├── Auth/                      # JWT, Password Hash
│   └── Migrations/                # EF Core migrations
│
├── APIRelatorios.IOC/             # Dependency Injection
│   └── DependencyInjection.cs     # Registro de serviços
│
├── APIRelatorios.WebAPI/          # Controllers HTTP
│   ├── Controllers/               # API endpoints
│   └── Program.cs                 # Configuração da app
│
└── docs/                          # 📄 Esta documentação
```

---

## 🔐 Autenticação

Todos os endpoints (exceto `/login`) requerem um **token JWT Bearer**.

**Como funciona:**

1. **Login** → POST `/login` com credenciais
2. **Recebe token** → Valid por `ExpireDays` dias
3. **Requisição** → Header: `Authorization: Bearer <token>`

**Usuário padrão:**
```
Login: admin
Senha: 123456
```

⚠️ **Altere em produção!**

---

## ❌ Tratamento de Erros

A API retorna erros estruturados:

```json
{
  "message": "Descrição do erro",
  "statusCode": 400,
  "timestamp": "2024-05-23T10:30:00Z"
}
```

| Código | Significado |
|--------|-------------|
| 200 | ✅ Sucesso |
| 400 | ❌ Requisição inválida |
| 401 | 🔐 Não autenticado |
| 403 | 🚫 Não autorizado |
| 404 | ❓ Não encontrado |
| 500 | ⚠️ Erro interno |

---

## 📝 Próximas Leituras

1. **Quer entender a arquitetura?** → [ARCHITECTURE.md](./ARCHITECTURE.md)
2. **Quer configurar o projeto?** → [SETUP.md](./SETUP.md)
3. **Quer usar a API?** → [API.md](./API.md)
4. **Quer entender os fluxos?** → [FLOWS.md](./FLOWS.md)
5. **Quer ver o banco de dados?** → [DATABASE.md](./DATABASE.md)

---

## 📞 Suporte

- 💬 Issues no GitHub
- 📧 Email: [seu-email]
- 🔗 Wiki: [link-wiki]

---

**Última atualização:** 23/05/2024  
**Versão:** 1.0.0
