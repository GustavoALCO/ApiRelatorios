# 🚀 Setup & Configuração - API Relatórios

## 📦 Pré-requisitos

### Obrigatório
- **Docker + Docker Compose** 20.10+
- **Git** 2.30+

### Opcional (Desenvolvimento Local)
- **.NET 10 SDK**
- **PostgreSQL 15** cliente (psql)
- **VS Code** ou **Visual Studio 2022+**
- **Postman** ou **Insomnia** (testar API)

---

## 🔧 Configuração Inicial

### 1. Clone o Repositório

```bash
git clone https://github.com/seu-usuario/ApiRelatorios.git
cd ApiRelatorios
```

### 2. Configure Variáveis de Ambiente

#### Opção A: Usar Docker (Recomendado)

Crie um arquivo `.env` na raiz:

```env
# Banco de dados
POSTGRES_USER=admin
POSTGRES_PASSWORD=senha123
POSTGRES_DB=api_relatorios
DB_HOST=postgres
DB_PORT=5432

# API
API_PORT=7000
API_ENVIRONMENT=Development

# JWT
JWT_KEY=sua_chave_super_secreta_com_minimo_32_caracteres
JWT_ISSUER=ApiRelatorios
JWT_AUDIENCE=ApiRelatoriosFrontend
JWT_EXPIRE_DAYS=7

# Azure
AZURE_MAPS_API_KEY=sua_chave_do_azure_maps
AZURE_STORAGE_CONNECTION=DefaultEndpointsProtocol=https;...
AZURE_STORAGE_CONTAINER=testes

# Certificados SSL
CERT_PATH=/app/certs/cert.pem
KEY_PATH=/app/certs/key.pem
```

#### Opção B: appsettings.json (Desenvolvimento Local)

Crie `appsettings.json` na raiz (já está em `.gitignore`):

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "Host": "localhost",
    "Port": "5432",
    "Database": "api_relatorios",
    "Username": "admin",
    "Password": "senha123"
  },
  "BlobConnection": {
    "ConnectionString": "DefaultEndpointsProtocol=https;...",
    "Container": "testes"
  },
  "Azurekey": {
    "AzureMapsApiKey": "sua_chave_azure_maps"
  },
  "Jwt": {
    "Key": "sua_chave_super_secreta_com_minimo_32_caracteres",
    "Issuer": "ApiRelatorios",
    "Audience": ["ApiRelatoriosFrontend"],
    "ExpireDays": 7
  },
  "AllowedHosts": "*"
}
```

---

## 🐳 Rodar com Docker

### Iniciar Tudo

```bash
docker compose up --build
```

Primeira execução demora (download de imagens, migrations).

**Output esperado:**
```
...
api_relatarios_1  | Now listening on: https://localhost:7000
api_relatarios_1  | Application started.
...
```

### Verificar Status

```bash
docker compose ps
```

### Acessar Serviços

| Serviço | URL |
|---------|-----|
| **API** | https://localhost:7000 |
| **Swagger** | https://localhost:7000/swagger |
| **PostgreSQL** | localhost:5432 |

### Parar Containers

```bash
docker compose down
```

### Ver Logs

```bash
docker compose logs -f api_relatarios
docker compose logs -f postgres
```

---

## 🛠️ Desenvolvimento Local (Sem Docker)

### 1. Instale .NET 10

```bash
# macOS (Homebrew)
brew install dotnet

# Windows
# Baixar em: https://dotnet.microsoft.com/download

# Linux (Ubuntu/Debian)
wget https://dot.net/v1/dotnet-install.sh
sudo bash dotnet-install.sh --channel 10.0
```

### 2. Instale PostgreSQL

```bash
# macOS
brew install postgresql@15

# Windows
# Baixar em: https://www.postgresql.org/download/windows/

# Linux (Ubuntu/Debian)
sudo apt-get install postgresql postgresql-contrib
```

### 3. Configure Banco de Dados

```bash
# Conecte ao PostgreSQL
psql -U postgres

# Crie banco de dados
CREATE DATABASE api_relatorios;
CREATE USER admin WITH PASSWORD 'senha123';
GRANT ALL PRIVILEGES ON DATABASE api_relatorios TO admin;

# Saia
\q
```

### 4. Atualize Migrations

```bash
cd APIRelatorios.WebAPI

dotnet ef database update
```

### 5. Rode a API

```bash
dotnet run
```

**Output esperado:**
```
info: Microsoft.AspNetCore.Hosting...
      Now listening on: https://localhost:7000
      Application started. Press Ctrl+C to shut down.
```

---

## 🔑 Chaves e Certificados

### Certificados SSL

Para desenvolvimento, criar self-signed:

```bash
# Criar pasta certs
mkdir certs

# Gerar certificado
openssl req -x509 -nodes -days 365 -newkey rsa:2048 \
  -keyout certs/key.pem \
  -out certs/cert.pem \
  -subj "/C=BR/ST=SP/L=SaoPaulo/O=Dev/CN=localhost"
```

⚠️ **Em produção:** Use certificados válidos (Let's Encrypt)

### JWT Secret Key

Gere uma chave segura:

```bash
# Node.js
node -e "console.log(require('crypto').randomBytes(32).toString('hex'))"

# Python
python3 -c "import secrets; print(secrets.token_hex(32))"

# Bash
openssl rand -hex 32
```

---

## 🧪 Testando a API

### 1. Login

```bash
curl -X POST https://localhost:7000/login \
  -H "Content-Type: application/json" \
  -d '{
    "login": "admin",
    "password": "123456"
  }' \
  --insecure
```

**Resposta:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "expiresIn": 7,
  "userType": "Admin"
}
```

### 2. Copiar Token

```bash
# Substitua abaixo:
TOKEN=eyJhbGciOiJIUzI1NiIs...
```

### 3. Listar Rotas

```bash
curl -X GET https://localhost:7000/Rotas \
  -H "Authorization: Bearer $TOKEN" \
  --insecure
```

### 4. Criar Rota

```bash
curl -X POST https://localhost:7000/Rotas \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "nomeRota": "Rota Teste",
    "alimentador": "Ali-001",
    "concessionaria": 1,
    "dataInicio": "2024-05-23T08:00:00Z"
  }' \
  --insecure
```

### 5. Via Swagger (Mais Fácil)

Acesse: https://localhost:7000/swagger

- Clique em "Authorize"
- Cole o token
- Use a UI para testar endpoints

---

## 📊 Estrutura de Diretórios

```
ApiRelatorios/
├── APIRelatorios.WebAPI/          # Controllers (entry point)
│   ├── Controllers/
│   ├── Program.cs
│   └── appsettings.json
│
├── APIRelatorios.Application/     # Handlers (CQRS)
│   ├── Features/Commands/
│   ├── Features/Queries/
│   └── Services/
│
├── APIRelatorios.Dommain/         # Entities & Interfaces
│   ├── Entities/
│   └── Interfaces/
│
├── APIRelatorios.Infra/           # Database & External
│   ├── Database/
│   ├── Repository/
│   ├── Auth/
│   └── Migrations/
│
├── APIRelatorios.IOC/             # Dependency Injection
│   └── DependencyInjection.cs
│
├── APIRelatorios.Testes/          # Unit Tests
│
├── docs/                          # 📚 Documentação
│   ├── README.md
│   ├── ARCHITECTURE.md
│   ├── API.md
│   ├── FLOWS.md
│   └── DATABASE.md
│
├── docker-compose.yml             # Orquestração Docker
├── Dockerfile                     # Imagem da API
├── .gitignore
├── appsettings.example.json       # Template config
└── certs/                         # SSL certificates
```

---

## 🐛 Troubleshooting

### "Connection refused" (PostgreSQL)

```bash
# Verificar se Postgres está rodando
docker compose ps

# Logs do Postgres
docker compose logs postgres

# Reiniciar
docker compose restart postgres
```

### "Invalid certificate" (SSL)

```bash
# Chrome/Edge: ignore (desenvolvimento)
# Ou crie novo certificado:
rm -rf certs/
# E execute docker compose up novamente
```

### "JWT token expired"

```bash
# Gere novo token via /login
curl -X POST https://localhost:7000/login \
  -H "Content-Type: application/json" \
  -d '{"login":"admin","password":"123456"}' \
  --insecure
```

### "Migrations pending"

```bash
# Aplicar migrations
dotnet ef database update

# Ou via Docker
docker compose exec api_relatorios \
  dotnet ef database update
```

---

## 📝 Variáveis Importantes

| Variável | Padrão | Descrição |
|----------|--------|-----------|
| `ASPNETCORE_ENVIRONMENT` | Production | Development, Staging, Production |
| `ASPNETCORE_URLS` | https://+:7000 | Portas de escuta |
| `ConnectionStrings:Host` | localhost | Host PostgreSQL |
| `Jwt:ExpireDays` | 7 | Dias de expiração token |
| `AZURE_MAPS_API_KEY` | - | Obrigatório para geolocalização |

---

## 🔒 Segurança em Produção

### Checklist

- [ ] Altere credencial padrão (admin/123456)
- [ ] Use certificado SSL válido (não self-signed)
- [ ] Configure JWT_KEY forte (32+ chars)
- [ ] Use HTTPS obrigatoriamente
- [ ] Configure CORS corretamente
- [ ] Ative logging estruturado
- [ ] Configure rate limiting
- [ ] Use variáveis de ambiente (não hardcode secrets)
- [ ] Atualize dependências NuGet
- [ ] Configure backup automático do BD

### Exemplo Deployment

```yaml
# docker-compose.production.yml
version: '3.8'
services:
  api_relatorios:
    image: seu-registro/api-relatorios:v1.0.0
    environment:
      ASPNETCORE_ENVIRONMENT: Production
      Jwt__Key: ${JWT_KEY}
      ConnectionStrings__Password: ${DB_PASSWORD}
    ports:
      - "443:7000"
    restart: always
    
  postgres:
    image: postgres:15
    environment:
      POSTGRES_PASSWORD: ${DB_PASSWORD}
    volumes:
      - pg_data:/var/lib/postgresql/data
    restart: always

volumes:
  pg_data:
```

---

## 📚 Próximos Passos

1. **Explorar Swagger:** https://localhost:7000/swagger
2. **Ler [ARCHITECTURE.md](./ARCHITECTURE.md)** - Entender código
3. **Ler [API.md](./API.md)** - Usar endpoints
4. **Ler [FLOWS.md](./FLOWS.md)** - Entender negócio
5. **Ler [DATABASE.md](./DATABASE.md)** - Schema e queries

---

## 📞 Suporte

- **Documentação:** Veja a pasta `/docs`
- **Issues:** GitHub Issues
- **Email:** seu-email@dominio.com

---

**Última atualização:** 23/05/2024  
**Versão:** 1.0.0
