# 🗄️ Banco de Dados - API Relatórios

## 📊 Visão Geral

**SGBD:** PostgreSQL 15+  
**ORM:** Entity Framework Core 8  
**Migrations:** Controladas via EF Core  

---

## 🗂️ Schema Relacional

```
┌──────────────────┐
│      users       │
├──────────────────┤
│ user_id (PK)     │
│ login (UNIQUE)   │
│ name             │
│ last_name        │
│ password_hash    │
│ salt             │
│ is_admin         │
│ created_at       │
└────────┬─────────┘
         │ 1:N
         │
    ┌────┴────────────────────────┐
    │                             │
    ▼                             ▼
┌─────────────────────┐    ┌─────────────────────┐
│   usuario_rotas     │    │      rotas          │
├─────────────────────┤    ├─────────────────────┤
│ usuario_rota_id(PK) │    │ rota_id (PK)        │
│ user_id (FK)        │    │ nome_rota           │
│ rota_id (FK)        │    │ alimentador         │
│ created_at          │    │ concessionaria      │
└─────────────────────┘    │ km                  │
                           │ data_inicio         │
                           │ data_final          │
                           └────────┬────────────┘
                                    │ 1:N
                                    │
                    ┌───────────────┴───────────────┐
                    │                               │
                    ▼                               ▼
         ┌──────────────────────┐      ┌──────────────────────┐
         │   evidencia_rota     │      │    imagens_rotas     │
         ├──────────────────────┤      ├──────────────────────┤
         │ evidencia_id (PK)    │      │ imagem_rota_id (PK)  │
         │ rota_id (FK)         │      │ rota_id (FK)         │
         │ fiscal_id (FK)       │      │ imagem_url           │
         │ checklist_id (FK)    │      │ created_at           │
         │ alimentador          │      └──────────────────────┘
         │ identificacao        │
         │ descricao            │
         │ endereco             │
         │ cidade               │
         │ latitude             │
         │ longitude            │
         │ horario              │
         │ emergencial          │
         │ is_valid             │
         └──────────┬───────────┘
                    │ 1:N
                    │
                    ▼
         ┌──────────────────────┐
         │    image_data        │
         ├──────────────────────┤
         │ image_id (PK)        │
         │ evidencia_id (FK)    │
         │ imagem_url           │
         │ created_at           │
         └──────────────────────┘
         
         ┌──────────────────────┐
         │   checklist          │
         ├──────────────────────┤
         │ checklist_id (PK)    │
         │ tema                 │
         │ sub_temas (array)    │
         └──────────────────────┘
```

---

## 📋 Tabelas Detalhadas

### `users`
Armazena usuários (fiscais e admins) do sistema.

| Coluna | Tipo | Constraints | Descrição |
|--------|------|-------------|-----------|
| `user_id` | SERIAL | PK, AUTO_INCREMENT | ID único |
| `login` | VARCHAR(50) | NOT NULL, UNIQUE | Nome de login |
| `name` | VARCHAR(100) | NOT NULL | Primeiro nome |
| `last_name` | VARCHAR(100) | NOT NULL | Sobrenome |
| `password_hash` | VARCHAR(255) | NOT NULL | Senha hashada (bcrypt) |
| `salt` | VARCHAR(255) | NOT NULL | Salt para hash |
| `is_admin` | BOOLEAN | DEFAULT false | É administrador? |
| `created_at` | TIMESTAMP | DEFAULT NOW() | Quando foi criado |

**Índices:**
```sql
CREATE INDEX idx_users_login ON users(login);
CREATE INDEX idx_users_admin ON users(is_admin);
```

**Exemplo:**
```sql
INSERT INTO users (login, name, last_name, password_hash, salt, is_admin)
VALUES ('joao.silva', 'João', 'Silva', 
        '$2a$12$...hashed...', 'salt123', false);
```

---

### `rotas`
Armazena as rotas de inspeção.

| Coluna | Tipo | Constraints | Descrição |
|--------|------|-------------|-----------|
| `rota_id` | UUID | PK | ID único |
| `nome_rota` | VARCHAR(200) | NOT NULL | Nome descritivo |
| `alimentador` | VARCHAR(50) | NOT NULL | Código alimentador |
| `concessionaria` | SMALLINT | NOT NULL | ID enum |
| `km` | DECIMAL(10,2) | NULL | KM percorridos |
| `data_inicio` | TIMESTAMP | NOT NULL | Quando começou |
| `data_final` | TIMESTAMP | NULL | Quando encerrou |

**Índices:**
```sql
CREATE INDEX idx_rotas_status ON rotas(data_final);
CREATE INDEX idx_rotas_data ON rotas(data_inicio);
```

**Enumeração concessionaria:**
```
0 = AES
1 = Enel
2 = Copel
3 = EDP
```

---

### `usuario_rotas`
Junction table: Relaciona usuários com rotas.

| Coluna | Tipo | Constraints | Descrição |
|--------|------|-------------|-----------|
| `usuario_rota_id` | UUID | PK | ID único |
| `user_id` | INT | FK, NOT NULL | Referência a users |
| `rota_id` | UUID | FK, NOT NULL | Referência a rotas |
| `created_at` | TIMESTAMP | DEFAULT NOW() | Quando foi atribuído |

**Foreign Keys:**
```sql
ALTER TABLE usuario_rotas
ADD CONSTRAINT fk_usuario_rotas_user
FOREIGN KEY (user_id) REFERENCES users(user_id);

ALTER TABLE usuario_rotas
ADD CONSTRAINT fk_usuario_rotas_rota
FOREIGN KEY (rota_id) REFERENCES rotas(rota_id)
ON DELETE CASCADE;
```

---

### `evidencia_rota`
Armazena as evidências (inspeções) de uma rota.

| Coluna | Tipo | Constraints | Descrição |
|--------|------|-------------|-----------|
| `evidencia_id` | UUID | PK | ID único |
| `rota_id` | UUID | FK, NOT NULL | Qual rota |
| `fiscal_id` | INT | FK, NOT NULL | Qual fiscal |
| `checklist_id` | UUID | FK, NULL | Referência a checklist |
| `alimentador` | VARCHAR(50) | NOT NULL | Alimentador inspecionado |
| `identificacao` | VARCHAR(100) | NULL | Ex: "Transformador T-001" |
| `descricao` | TEXT | NULL | Observações |
| `endereco` | VARCHAR(255) | NOT NULL | Logradouro |
| `cidade` | VARCHAR(100) | NOT NULL | Município |
| `latitude` | DECIMAL(10,8) | NOT NULL | Coordenada |
| `longitude` | DECIMAL(11,8) | NOT NULL | Coordenada |
| `horario` | TIMESTAMP | NOT NULL | Quando foi capturada |
| `emergencial` | BOOLEAN | DEFAULT false | Caso urgente? |
| `is_valid` | BOOLEAN | DEFAULT true | Ativa? |
| `created_at` | TIMESTAMP | DEFAULT NOW() | Quando foi criada |

**Foreign Keys:**
```sql
ALTER TABLE evidencia_rota
ADD CONSTRAINT fk_evidencia_rota_user
FOREIGN KEY (fiscal_id) REFERENCES users(user_id);

ALTER TABLE evidencia_rota
ADD CONSTRAINT fk_evidencia_rota_rota
FOREIGN KEY (rota_id) REFERENCES rotas(rota_id)
ON DELETE CASCADE;
```

**Índices:**
```sql
CREATE INDEX idx_evidencia_rota ON evidencia_rota(rota_id);
CREATE INDEX idx_evidencia_fiscal ON evidencia_rota(fiscal_id);
CREATE INDEX idx_evidencia_horario ON evidencia_rota(horario);
```

---

### `image_data`
Armazena referências para imagens no Azure Blob Storage.

| Coluna | Tipo | Constraints | Descrição |
|--------|------|-------------|-----------|
| `image_id` | UUID | PK | ID único |
| `evidencia_id` | UUID | FK, NOT NULL | Qual evidência |
| `imagem_url` | VARCHAR(500) | NOT NULL | URL no Azure Blob |
| `created_at` | TIMESTAMP | DEFAULT NOW() | Quando foi upload |

**Foreign Key:**
```sql
ALTER TABLE image_data
ADD CONSTRAINT fk_image_data_evidencia
FOREIGN KEY (evidencia_id) REFERENCES evidencia_rota(evidencia_id)
ON DELETE CASCADE;
```

---

### `checklist`
Armazena os temas e subtemas de fiscalização.

| Coluna | Tipo | Constraints | Descrição |
|--------|------|-------------|-----------|
| `checklist_id` | UUID | PK | ID único |
| `tema` | SMALLINT | NOT NULL | Tema (enum) |
| `sub_temas` | SMALLINT[] | NOT NULL | Array de subtemas |

**Enumeração tema:**
```
0 = Segurança
1 = Qualidade
2 = Regularidade
3 = Manutenção
```

**Enumeração sub_temas:**
```
0 = Via
1 = Transformador
2 = Chave Seccionadora
3 = Isolador
4 = Cabo
5 = Poste
```

---

## 🔍 Queries Comuns

### Listar rotas ativas
```sql
SELECT r.rota_id, r.nome_rota, r.alimentador, 
       COUNT(e.evidencia_id) as total_evidencias
FROM rotas r
LEFT JOIN evidencia_rota e ON r.rota_id = e.rota_id
WHERE r.data_final IS NULL
GROUP BY r.rota_id
ORDER BY r.data_inicio DESC;
```

### Evidências por rota
```sql
SELECT e.evidencia_id, e.horario, u.name, u.last_name,
       COUNT(i.image_id) as total_imagens
FROM evidencia_rota e
JOIN users u ON e.fiscal_id = u.user_id
LEFT JOIN image_data i ON e.evidencia_id = i.evidencia_id
WHERE e.rota_id = $1
GROUP BY e.evidencia_id, u.user_id
ORDER BY e.horario DESC;
```

### Fiscais por rota
```sql
SELECT DISTINCT u.user_id, u.login, u.name, u.last_name
FROM usuario_rotas ur
JOIN users u ON ur.user_id = u.user_id
WHERE ur.rota_id = $1;
```

### Estatísticas de evidências
```sql
SELECT c.tema, COUNT(*) as total,
       AVG(COUNT(*)) OVER() as media_por_tema
FROM evidencia_rota e
JOIN checklist c ON e.checklist_id = c.checklist_id
WHERE EXTRACT(MONTH FROM e.created_at) = $1
GROUP BY c.tema;
```

---

## 🔐 Backup e Restore

### Backup completo
```bash
pg_dump -U postgres -d api_relatorios -v \
  -f backup_$(date +%Y%m%d).sql
```

### Restore
```bash
psql -U postgres -d api_relatorios < backup_20240523.sql
```

### Backup apenas dados
```bash
pg_dump -U postgres -d api_relatorios --data-only \
  -f dados_$(date +%Y%m%d).sql
```

---

## 📈 Performance

### Índices Críticos
```sql
-- Já criados:
CREATE INDEX idx_rotas_status ON rotas(data_final);
CREATE INDEX idx_evidencia_rota ON evidencia_rota(rota_id);
CREATE INDEX idx_evidencia_fiscal ON evidencia_rota(fiscal_id);

-- Considerar futuros:
CREATE INDEX idx_evidencia_horario ON evidencia_rota(horario);
CREATE INDEX idx_image_data_evidencia ON image_data(evidencia_id);
```

### Estatísticas
```sql
-- Atualizar estatísticas (VACUUM)
VACUUM ANALYZE rotas;
VACUUM ANALYZE evidencia_rota;
VACUUM ANALYZE image_data;
```

---

## 🔄 Migrations

Controladas via **Entity Framework Core**.

### Ver migrations
```bash
dotnet ef migrations list
```

### Criar migration
```bash
dotnet ef migrations add NomeDaMigracao
```

### Aplicar migrations
```bash
dotnet ef database update
```

### Rollback
```bash
dotnet ef database update NomedaMigracaoAnterior
```

---

## ⚠️ Constraints e Regras

| Regra | Nível | Descrição |
|-------|-------|-----------|
| Rota não pode ser deletada se tem evidências | DB | FK ON DELETE CASCADE |
| Fiscal não pode ser deletado se atribuído | DB | FK constraint |
| Evidência requer Rota e Fiscal válidos | DB | NOT NULL FK |
| Latitude entre -90 e 90 | App | Validação EF Core |
| Longitude entre -180 e 180 | App | Validação EF Core |
| Rota finalizada não pode receber evidências | App | Validação Handler |
| Senha hasheada com salt único | App | Serviço de hash |

---

**Próxima leitura:** [SETUP.md](./SETUP.md)
