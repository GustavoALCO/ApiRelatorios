# 🔄 Fluxos de Negócio - API Relatórios

## 📚 Índice de Fluxos

1. [Criação de Rota](#1-criação-de-rota)
2. [Criação de Evidência](#2-criação-de-evidência)
3. [Adição de Fiscais](#3-adição-de-fiscais)
4. [Finalização de Rota](#4-finalização-de-rota)
5. [Geração de Relatório](#5-geração-de-relatório)
6. [Autenticação](#6-autenticação)

---

## 1️⃣ Criação de Rota

**Objetivo:** Iniciar uma nova rota de inspeção

**Atores:** Supervisor/Admin

**Pré-requisitos:**
- Usuário autenticado com role Admin
- Alimentador válido
- Concessionária válida

**Fluxo Principal:**

```
1. Supervisor acessa sistema
   ↓
2. Clica em "Nova Rota"
   ↓
3. Preenche:
   - Nome: "Rota Zona Centro"
   - Alimentador: "Ali-001"
   - Concessionária: "Enel"
   - Data/Hora início
   ↓
4. Sistema valida dados (FluentValidation)
   ├─ Nome obrigatório?
   ├─ Alimentador existe?
   └─ Data válida?
   ↓
5. CreateRotaHandler orquestra:
   ├─ Cria objeto Rota no domínio
   ├─ Define status = "Ativa"
   ├─ Inicializa coleções (Fiscais, Imagens)
   └─ Persiste no PostgreSQL
   ↓
6. Rota criada com sucesso
   └─ Retorna rotaId
```

**Dados Criados:**
```sql
INSERT INTO rotas (rotaId, nomeRota, alimentador, 
                   concessionaria, dataInicio, dataFinal)
VALUES ('550e8400...', 'Rota Zona Centro', 'Ali-001', 
        1, '2024-05-23 08:00:00', NULL)
```

**Fluxo de Erro:**
```
Se alimentador inválido
  → ValidationException
  → 400 Bad Request
  → "Alimentador não encontrado"
```

---

## 2️⃣ Criação de Evidência

**Objetivo:** Registrar uma inspeção com fotos e checklist

**Atores:** Fiscal (via app mobile)

**Pré-requisitos:**
- Rota ativa (DataFinal = NULL)
- Fiscal atribuído à rota
- Mínimo 1 foto
- GPS ativo

**Fluxo Principal:**

```
ETAPA 1: CAPTURA
┌─────────────────────────────────────────┐
│ Fiscal abre app e tira fotos            │
│ ├─ Locação GPS capturada automaticamente │
│ ├─ Horário capturado                    │
│ ├─ Seleciona Tema: "Segurança"         │
│ ├─ Seleciona SubTemas: ["Via", "Poste"]│
│ └─ Digita descrição (opcional)          │
└──────────────┬──────────────────────────┘
               │
ETAPA 2: VALIDAÇÃO CLIENTE
┌──────────────────────────────────────────┐
│ App valida localmente:                  │
│ ├─ Fotos em Base64?                     │
│ ├─ GPS válido (lat/lon)?                │
│ └─ Tema válido?                         │
└──────────────┬───────────────────────────┘
               │
ETAPA 3: ENVIO PARA API
┌──────────────────────────────────────────┐
│ POST /EvidenciaRotas                    │
│ {                                       │
│   evidenciaId: "660e8400-...",         │
│   rotaID: "550e8400-...",              │
│   fiscalId: 1,                         │
│   base64: ["iVBORw0KG..."],            │
│   latitude: -23.5505,                  │
│   longitude: -46.6333,                 │
│   temaFiscalizacao: 0,                 │
│   subTemaFiscalizacao: [0, 5]          │
│ }                                       │
└──────────────┬───────────────────────────┘
               │
ETAPA 4: VALIDAÇÃO SERVER
┌──────────────────────────────────────────┐
│ CreateEvidenciaHandler.Handler():        │
│                                         │
│ [1] Valida Rota                         │
│     ├─ Rota existe?                     │
│     ├─ DataFinal == NULL?               │
│     └─ Status = Ativa?                  │
│                                         │
│ [2] Valida Fiscal                       │
│     ├─ Fiscal existe?                   │
│     ├─ Fiscal atribuído à Rota?         │
│     └─ Fiscal ativo?                    │
│                                         │
│ [3] Valida Imagens                      │
│     ├─ Base64 válido?                   │
│     └─ Mínimo 1 imagem?                 │
└──────────────┬───────────────────────────┘
               │
ETAPA 5: ENRIQUECIMENTO DE DADOS
┌──────────────────────────────────────────┐
│ [1] Preenche Alimentador                │
│     ├─ Se vazio, copia da Rota          │
│     ├─ Resultado: \"Ali-001\"            │
│     └─ Log: \"Alimentador atribuido\"    │
│                                         │
│ [2] Busca Endereço + Cidade             │
│     ├─ Azure Maps Reverse Geocoding     │
│     │  (lat=-23.5505, lon=-46.6333)     │
│     │                                   │
│     ├─ Retorna:                         │
│     │  • Rua: \"Rua das Flores, 123\"    │
│     │  • Cidade: \"São Paulo\"           │
│     │  • CEP: \"01000-000\"              │
│     │                                   │
│     └─ Atribui ao comando                │
│                                         │
│ [3] Cria CheckList                      │
│     ├─ Tema: 0 (Segurança)              │
│     ├─ SubTemas: [0, 5]                 │
│     └─ SubTemaAlimentadores enum        │
└──────────────┬───────────────────────────┘
               │
ETAPA 6: UPLOAD DE IMAGENS
┌──────────────────────────────────────────┐
│ ISavedImages.UploadListBase64ImagesAsync│
│                                         │
│ Para cada imagem:                       │
│ ├─ Decodifica Base64 → Bytes            │
│ ├─ Calcula hash (UUID)                  │
│ ├─ Organiza path:                       │
│ │  /Ali-001/Joao_Silva/2024-05-23_10h30│
│ │  /evidencia_660e8400.jpg              │
│ │                                       │
│ ├─ Upload para Azure Blob Storage       │
│ ├─ Retorna URL pública                  │
│ │  https://blob.azure.com/...           │
│ │                                       │
│ └─ Lista de URLs                        │
└──────────────┬───────────────────────────┘
               │
ETAPA 7: CRIAR ENTIDADES
┌──────────────────────────────────────────┐
│ CheckList checkList = new CheckList(    │
│   id: evidenciaId,                      │
│   tema: TemaCheck.Segurança,            │
│   subTemas: [Via, Poste]                │
│ );                                       │
│                                         │
│ EvidenciaRota evidencia = new(          │
│   evidenciaRotaId: \"660e8400-...\",    │
│   rotaID: \"550e8400-...\",             │
│   fiscalId: 1,                          │
│   checkList: checkList,                 │
│   alimentador: \"Ali-001\",              │
│   descricao: \"Inspeção OK\",            │
│   images: [\"https://blob.azure.com/..\"],
│   endereco: \"Rua das Flores, 123\",     │
│   cidade: \"São Paulo\",                 │
│   latitude: -23.5505,                   │
│   longitude: -46.6333,                  │
│   horario: \"2024-05-23 07:30:00\" (UTC-3), │
│   emergencial: false,                   │
│   IsValid: true                         │
│ );                                       │
└──────────────┬───────────────────────────┘
               │
ETAPA 8: PERSISTÊNCIA
┌──────────────────────────────────────────┐
│ EvidenciaRotaCommands.SaveImage():      │
│                                         │
│ DbContext.EvidenciaRota.AddAsync(evi)  │
│ DbContext.SaveChangesAsync()            │
│                                         │
│ Inserts no PostgreSQL:                  │
│ ├─ INSERT INTO evidencia_rota (...)    │
│ ├─ INSERT INTO checklist (...)         │
│ └─ INSERT INTO image_data (...)        │
└──────────────┬───────────────────────────┘
               │
ETAPA 9: RESPOSTA
┌──────────────────────────────────────────┐
│ 200 OK                                  │
│ {                                       │
│   \"message\": \"Evidência criada\",     │
│   \"evidenciaId\": \"660e8400-...\",    │
│   \"imagens\": 3                        │
│ }                                       │
└──────────────────────────────────────────┘
```

**Fluxos de Erro:**

```
CENÁRIO 1: Rota finalizada
└─ DataFinal != NULL
└─ Exception: "Rota Já finalizada"
└─ 400 Bad Request

CENÁRIO 2: Fiscal não existe
└─ IUserQuery.BuscarFiscalId() = null
└─ Exception: "Erro ao encontrar fiscal"
└─ 400 Bad Request

CENÁRIO 3: Upload falha
└─ Azure Blob Storage offline
└─ Exception: "Erro ao fazer upload da imagem"
└─ 500 Internal Server Error
└─ Evidência NÃO é persistida (rollback)

CENÁRIO 4: Validação de tema inválida
└─ temaFiscalizacao = 99 (enum não existe)
└─ FluentValidation rejeita
└─ 422 Unprocessable Entity
```

---

## 3️⃣ Adição de Fiscais

**Objetivo:** Atribuir fiscais responsáveis à rota

**Atores:** Supervisor

**Fluxo:**

```
Supervisor seleciona Rota
         ↓
Clica "Adicionar Fiscais"
         ↓
Checkboxes aparecem (lista de fiscais)
         ↓
Seleciona: João Silva (1), Maria Santos (2)
         ↓
PATCH /Rotas/AddFiscais
{
  "rotaId": "550e8400-...",
  "fiscalIds": [1, 2]
}
         ↓
AddFiscalRotaHandler valida:
├─ Rota existe?
├─ Rota aberta?
└─ Fiscais existem?
         ↓
Para cada fiscal:
├─ Cria UsuarioRota linking
├─ Adiciona a Rota.Fiscais
└─ INSERT INTO usuario_rotas
         ↓
200 OK - Fiscais adicionados
```

---

## 4️⃣ Finalização de Rota

**Objetivo:** Encerrar uma rota e calcular km total

**Atores:** Supervisor

**Fluxo:**

```
Supervisor encerra expediente
         ↓
POST /Rotas/FinalizarRota
{
  "rotaId": "550e8400-...",
  "kmPercorrido": 28.3,
  "dataFinal": "2024-05-23T17:30:00Z"
}
         ↓
FinalizarRotaHandler orquestra:
├─ Valida Rota existe
├─ Valida Rota aberta (DataFinal == NULL)
├─ Chama método Rota.finalizandoRota()
│  └─ DataFinal = 2024-05-23T17:30:00Z
│  └─ Km = 28.3
│  └─ Status = "Finalizada"
└─ Persiste UPDATE
         ↓
UPDATE rotas
SET data_final = '2024-05-23 17:30:00',
    km = 28.3
WHERE rota_id = '550e8400-...'
         ↓
200 OK
{
  "message": "Rota finalizada com sucesso",
  "kmTotal": 28.3
}
```

---

## 5️⃣ Geração de Relatório

**Objetivo:** Gerar documento Word (.docx) com evidências

**Atores:** Supervisor

**Fluxo:**

```
Supervisor clica "Gerar Relatório"
         ↓
POST /Rotas/CriarRelatorio
{
  "rotaId": "550e8400-..."
}
         ↓
CreateRelatorioHandler orquestra:
├─ [1] Valida Rota
│      └─ Rota deve estar finalizada
│
├─ [2] Busca todas EvidenciaRota
│      └─ SELECT * FROM evidencia_rota
│         WHERE rota_id = '550e8400-...'
│
├─ [3] Agrupa evidências por tema
│      ├─ Segurança: 5 evidências
│      ├─ Qualidade: 3 evidências
│      └─ Manutenção: 2 evidências
│
├─ [4] Gera Word (.docx)
│      ├─ Cabeçalho (info da rota)
│      ├─ Sumário
│      ├─ Para cada evidência:
│      │  ├─ Tema
│      │  ├─ Data/Hora
│      │  ├─ Localização (lat/lon)
│      │  ├─ Descrição
│      │  ├─ Imagens (embed)
│      │  └─ CheckList
│      └─ Rodapé (assinatura, data)
│
├─ [5] Downlaod imagens de Azure Blob
│      └─ Para cada evidência.Images
│
├─ [6] Cria ZIP
│      ├─ Relatorio.docx
│      ├─ imagens/
│      │  ├─ evidencia_1.jpg
│      │  ├─ evidencia_2.jpg
│      │  └─ ...
│      └─ Emergencial.docx (se houver)
│
└─ [7] Retorna arquivo para download
         ↓
200 OK
Content-Type: application/zip
File: Relatorio.zip
```

**Fluxo de Erro:**
```
Se Rota ainda aberta (DataFinal == NULL)
└─ Exception: "Rota não finalizada"
└─ 400 Bad Request
```

---

## 6️⃣ Autenticação

**Objetivo:** Validar credenciais e emitir JWT

**Fluxo:**

```
Fiscal abre app
         ↓
Digita login e senha
         ↓
POST /login
{
  "login": "joao.silva",
  "password": "senha123"
}
         ↓
LoginHandler:
├─ [1] Busca usuário
│      └─ SELECT * FROM users WHERE login = ?
│
├─ [2] Valida senha
│      ├─ Busca salt do BD
│      ├─ Hash password com salt
│      ├─ Compara com hash_stored
│      └─ Match?
│
├─ [3] Gera JWT
│      ├─ Payload:
│      │  {
│      │    "sub": "1",
│      │    "login": "joao.silva",
│      │    "role": "User",
│      │    "iat": 1716470400,
│      │    "exp": 1717075200
│      │  }
│      ├─ Sign com chave privada (RS256)
│      └─ Token válido por 7 dias
│
└─ [4] Retorna resposta
         ↓
200 OK
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "expiresIn": 7,
  "userType": "User"
}
         ↓
App armazena token em flutter_secure_storage
         ↓
Para próximas requisições:
Header: Authorization: Bearer eyJhbGciOi...
```

**Fluxo de Erro:**

```
CENÁRIO 1: Login não existe
└─ SELECT retorna NULL
└─ 401 Unauthorized
└─ "Credenciais inválidas"

CENÁRIO 2: Senha errada
└─ Hash(input) != Hash(stored)
└─ 401 Unauthorized
└─ "Credenciais inválidas"

CENÁRIO 3: Token expirado
└─ Token.exp < CurrentTime
└─ 401 Unauthorized
└─ "Token expirado"
```

---

## 📊 Resumo de Estados

### Estados da Rota
```
[CRIADA]  → [ATIVA]  → [FINALIZADA]
  ↓         ↓ ✗       ↓
  │         Sem       Com
  │         evidências evidências
  │                   │
  └───────────────────┘
   (pode deletar)
```

### Estados da Evidência
```
[CRIADA] ─→ [ATIVA] ─→ [DESATIVADA]
            (válida)    (deletada logicamente)
```

---

**Próxima leitura:** [DATABASE.md](./DATABASE.md)
