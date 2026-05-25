# 🔌 API Reference - API Relatórios

## 📌 Base URL

```
Development:  https://localhost:7000
Production:   https://api.relatorios.com
```

## 🔐 Autenticação

Todos os endpoints (exceto `POST /login`) requerem:

```
Authorization: Bearer <jwt_token>
```

**Exemplo:**
```bash
curl -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIs..." \
     https://localhost:7000/rotas
```

---

## 📋 Endpoints

### 🔑 Autenticação

#### Login
```http
POST /login
Content-Type: application/json

{
  "login": "admin",
  "password": "123456"
}
```

**Response 200:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 7,
  "userType": "Admin"
}
```

**Response 401:**
```json
{
  "message": "Credenciais inválidas",
  "statusCode": 401
}
```

---

### 🛣️ Rotas

#### Listar Rotas com Filtros
```http
GET /Rotas?status=ativa&dataInicio=2024-05-01&dataFinal=2024-05-31
Authorization: Bearer <token>
```

**Query Parameters:**
| Parâmetro | Tipo | Descrição |
|-----------|------|-----------|
| `status` | string | `ativa`, `finalizada`, `emergencial` |
| `dataInicio` | date | Data de início (ISO 8601) |
| `dataFinal` | date | Data de fim (ISO 8601) |
| `alimentador` | string | Nome do alimentador |

**Response 200:**
```json
{
  "data": [
    {
      "rotaId": "550e8400-e29b-41d4-a716-446655440000",
      "nomeRota": "Rota Zona Centro",
      "alimentador": "Ali-001",
      "concessionaria": "Enel",
      "km": 25.5,
      "dataInicio": "2024-05-23T08:00:00Z",
      "dataFinal": null,
      "totalEvidencias": 15,
      "fiscaisAtribuidos": 3
    }
  ],
  "total": 1,
  "page": 1
}
```

---

#### Buscar Rota por ID
```http
GET /Rotas/{rotaId}
Authorization: Bearer <token>
```

**URL Parameters:**
| Parâmetro | Tipo | Descrição |
|-----------|------|-----------|
| `rotaId` | guid | ID da rota |

**Response 200:**
```json
{
  "rotaId": "550e8400-e29b-41d4-a716-446655440000",
  "nomeRota": "Rota Zona Centro",
  "alimentador": "Ali-001",
  "concessionaria": "Enel",
  "km": 25.5,
  "dataInicio": "2024-05-23T08:00:00Z",
  "dataFinal": null,
  "imagens": [
    {
      "evidenciaId": "660e8400-e29b-41d4-a716-446655440001",
      "quantidade": 12
    }
  ],
  "fiscais": [
    {
      "usuarioRotaId": "770e8400-e29b-41d4-a716-446655440002",
      "fiscalId": 1,
      "nome": "João Silva"
    }
  ]
}
```

---

#### Criar Rota
```http
POST /Rotas
Content-Type: application/json
Authorization: Bearer <token>

{
  "nomeRota": "Rota Zona Centro",
  "alimentador": "Ali-001",
  "concessionaria": 1,
  "dataInicio": "2024-05-23T08:00:00Z"
}
```

**Request Body:**
| Campo | Tipo | Obrigatório | Descrição |
|-------|------|-------------|-----------|
| `nomeRota` | string | ✅ | Nome da rota |
| `alimentador` | string | ✅ | Código alimentador |
| `concessionaria` | int | ✅ | ID concessionária (enum) |
| `dataInicio` | datetime | ✅ | Quando inicia |

**Response 201:**
```json
{
  "rotaId": "550e8400-e29b-41d4-a716-446655440000",
  "nomeRota": "Rota Zona Centro",
  "message": "Rota criada com sucesso"
}
```

---

#### Adicionar Fiscal à Rota
```http
PATCH /Rotas/AddFiscais
Content-Type: application/json
Authorization: Bearer <token>

{
  "rotaId": "550e8400-e29b-41d4-a716-446655440000",
  "fiscalIds": [1, 2, 3]
}
```

**Response 200:**
```json
{
  "message": "Fiscais adicionados com sucesso",
  "fiscaisAdicionados": 3
}
```

---

#### Atualizar Nome da Rota
```http
PATCH /Rotas/nome
Content-Type: application/json
Authorization: Bearer <token>

{
  "rotaId": "550e8400-e29b-41d4-a716-446655440000",
  "novoNome": "Rota Zona Centro - REVISÃO"
}
```

**Response 200:**
```json
{
  "message": "Rota atualizada com sucesso"
}
```

---

#### Finalizar Rota
```http
PATCH /Rotas/FinalizarRota
Content-Type: application/json
Authorization: Bearer <token>

{
  "rotaId": "550e8400-e29b-41d4-a716-446655440000",
  "kmPercorrido": 28.3,
  "dataFinal": "2024-05-23T17:30:00Z"
}
```

**Response 200:**
```json
{
  "message": "Rota finalizada com sucesso",
  "kmTotal": 28.3
}
```

---

#### Deletar Rota
```http
DELETE /Rotas/{rotaId}
Authorization: Bearer <token>
```

**Response 200:**
```json
{
  "message": "Rota deletada com sucesso"
}
```

---

### 📸 Evidências

#### Criar Evidência
```http
POST /EvidenciaRotas
Content-Type: application/json
Authorization: Bearer <token>

{
  "evidenciaId": "660e8400-e29b-41d4-a716-446655440001",
  "rotaID": "550e8400-e29b-41d4-a716-446655440000",
  "fiscalId": 1,
  "temaFiscalizacao": 1,
  "subTemaFiscalizacao": [2, 3],
  "identificacao": "Transformador T-001",
  "alimentador": "Ali-001",
  "descricao": "Inspeção de transformador, sem falhas encontradas",
  "base64": [
    "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNk+M9QDwADhgGAWjR9awAAAABJRU5ErkJggg=="
  ],
  "endereco": "Rua das Flores, 123",
  "cidade": "São Paulo",
  "latitude": -23.5505,
  "longitude": -46.6333,
  "horario": "2024-05-23T10:30:00Z",
  "emergencial": false
}
```

**Request Body:**
| Campo | Tipo | Obrigatório | Descrição |
|-------|------|-------------|-----------|
| `evidenciaId` | guid | ✅ | ID único da evidência |
| `rotaID` | guid | ✅ | ID da rota |
| `fiscalId` | int | ✅ | ID do fiscal |
| `temaFiscalizacao` | int | ✅ | Tema (enum) |
| `subTemaFiscalizacao` | int[] | ✅ | Subtemas (enum array) |
| `base64` | string[] | ✅ | Imagens em Base64 |
| `latitude` | float | ✅ | Coordenada |
| `longitude` | float | ✅ | Coordenada |
| `horario` | datetime | ✅ | Momento captura |
| `endereco` | string | ❌ | Auto-preenchido via Azure Maps |
| `cidade` | string | ❌ | Auto-preenchido via Azure Maps |
| `emergencial` | bool | ❌ | Caso urgente? |

**Response 200:**
```json
{
  "message": "Evidência criada com sucesso",
  "evidenciaId": "660e8400-e29b-41d4-a716-446655440001",
  "imagens": 1
}
```

---

#### Listar Evidências de uma Rota
```http
GET /EvidenciaRotas/TodasEvidencias?idRota=550e8400-e29b-41d4-a716-446655440000
Authorization: Bearer <token>
```

**Query Parameters:**
| Parâmetro | Tipo | Obrigatório |
|-----------|------|-------------|
| `idRota` | guid | ✅ |

**Response 200:**
```json
{
  "data": [
    {
      "evidenciaId": "660e8400-e29b-41d4-a716-446655440001",
      "fiscalId": 1,
      "fiscalNome": "João Silva",
      "tema": "Segurança",
      "subTemas": ["Via", "Transformador"],
      "descricao": "Inspeção OK",
      "cidade": "São Paulo",
      "horario": "2024-05-23T10:30:00Z",
      "imagens": 3,
      "emergencial": false,
      "ativo": true
    }
  ],
  "total": 15
}
```

---

#### Buscar Evidência por ID
```http
GET /EvidenciaRotas/Id?commands={evidenciaId}
Authorization: Bearer <token>
```

**Response 200:**
```json
{
  "evidenciaId": "660e8400-e29b-41d4-a716-446655440001",
  "rotaId": "550e8400-e29b-41d4-a716-446655440000",
  "fiscalId": 1,
  "tema": "Segurança",
  "imagens": [
    {
      "imageUrl": "https://blob.azure.com/evidencias/..."
    }
  ]
}
```

---

#### Atualizar Descrição de Evidência
```http
PATCH /EvidenciaRotas
Content-Type: application/json
Authorization: Bearer <token>

{
  "evidenciaId": "660e8400-e29b-41d4-a716-446655440001",
  "descricao": "Falha encontrada na isolação"
}
```

**Response 200:**
```json
{
  "message": "Evidência atualizada com sucesso"
}
```

---

#### Deletar Evidência
```http
DELETE /EvidenciaRotas?command={evidenciaId}
Authorization: Bearer <token>
```

**Response 200:**
```json
{
  "message": "Evidência deletada com sucesso"
}
```

---

### 👥 Fiscais/Usuários

#### Listar Todos Fiscais
```http
GET /Fiscals
Authorization: Bearer <token>
```

**Response 200:**
```json
{
  "data": [
    {
      "userId": 1,
      "login": "joao.silva",
      "nome": "João",
      "sobrenome": "Silva",
      "isAdmin": false,
      "criadoEm": "2024-01-15T08:00:00Z"
    }
  ],
  "total": 5
}
```

---

#### Criar Fiscal
```http
POST /Fiscals
Content-Type: application/json
Authorization: Bearer <token>

{
  "login": "maria.santos",
  "nome": "Maria",
  "sobrenome": "Santos",
  "senha": "Senha@123"
}
```

**Response 201:**
```json
{
  "userId": 2,
  "login": "maria.santos",
  "message": "Fiscal criado com sucesso"
}
```

---

### 📄 Relatórios

#### Gerar Relatório Word
```http
POST /Rotas/CriarRelatorio
Content-Type: application/json
Authorization: Bearer <token>

{
  "rotaId": "550e8400-e29b-41d4-a716-446655440000"
}
```

**Response 200:** (File download)
```
Content-Type: application/zip
Content-Disposition: attachment; filename="Relatorio.zip"

[Binary file]
```

**Conteúdo do ZIP:**
```
Relatorio.zip
├── Relatorio.docx         # Word com texto e tabelas
├── Emergencial.docx       # Se houver casos emergenciais
└── imagens/
    ├── evidencia_1.jpg
    ├── evidencia_2.jpg
    └── ...
```

---

#### Gerar Caso Emergencial
```http
POST /Rotas/CriarEmergencial
Content-Type: application/json
Authorization: Bearer <token>

{
  "rotaId": "550e8400-e29b-41d4-a716-446655440000",
  "descricaoEmergencia": "Perigo iminente"
}
```

**Response 200:** (File download)
```
Content-Type: application/zip
Content-Disposition: attachment; filename="Emergencial.zip"
```

---

## 📊 Enums

### Concessionarias
```
0 = AES
1 = Enel
2 = Copel
3 = EDP
```

### TemaCheck (Temas de Fiscalização)
```
0 = Segurança
1 = Qualidade
2 = Regularidade
3 = Manutenção
```

### SubTemaAlimentadores
```
0 = Via
1 = Transformador
2 = Chave Seccionadora
3 = Isolador
4 = Cabo
5 = Poste
```

---

## ❌ Códigos de Erro

| Código | Erro | Causa |
|--------|------|-------|
| 200 | OK | ✅ Sucesso |
| 201 | Created | ✅ Criado com sucesso |
| 400 | Bad Request | ❌ Dados inválidos |
| 401 | Unauthorized | 🔐 Token inválido/expirado |
| 403 | Forbidden | 🚫 Sem permissão |
| 404 | Not Found | ❓ Recurso não existe |
| 409 | Conflict | ⚠️ Conflito (ex: rota já finalizada) |
| 422 | Unprocessable Entity | ⚠️ Validação falhou |
| 500 | Internal Server Error | 💥 Erro no servidor |

---

## 🧪 Exemplos com cURL

### Login
```bash
curl -X POST https://localhost:7000/login \
  -H "Content-Type: application/json" \
  -d '{
    "login": "admin",
    "password": "123456"
  }'
```

### Listar Rotas
```bash
curl -X GET https://localhost:7000/Rotas \
  -H "Authorization: Bearer eyJhbGciOi..." \
  -H "Content-Type: application/json"
```

### Criar Rota
```bash
curl -X POST https://localhost:7000/Rotas \
  -H "Authorization: Bearer eyJhbGciOi..." \
  -H "Content-Type: application/json" \
  -d '{
    "nomeRota": "Rota Zona Centro",
    "alimentador": "Ali-001",
    "concessionaria": 1,
    "dataInicio": "2024-05-23T08:00:00Z"
  }'
```

---

## 📱 Swagger/OpenAPI

Acesse a documentação interativa:
```
https://localhost:7000/swagger/index.html
```

Ou JSON:
```
https://localhost:7000/swagger/v1/swagger.json
```

---

**Próxima leitura:** [FLOWS.md](./FLOWS.md)
