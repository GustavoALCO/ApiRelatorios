# Configuração da Api.

## Certificados

Os certificados não são versionados.

para a criação deles é necessario criar uma pasta

`mkdir certs`

e apos executar o comando
`docker run --rm -v ${PWD}/certs:/certs alpine sh -c "apk add --no-cache openssl && openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout /certs/key.pem -out /certs/cert.pem -subj '/C=BR/ST=SP/L=SaoPaulo/O=Dev/CN=localhost'"`

---

## AppSettings

O appsettings não é versionado.

para a criação deve seguir este modelo

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "Host": "**",
    "Port": "**",
    "Database": "**",
    "Username": "**",
    "Password": "**"
  },
  "BlobConnection": {
    "ConnectionString": "**",
    "Container": "**"
  },
  "Jwt": {
    "Key": "**",
    "Issuer": "**",
    "Audience": [
      "**"
    ],
    "ExpireDays": "**"
  },
  "AllowedHosts": "*"
}

```

---

## Subir a API

para subir a api na raiz do projeto deve escrever o comando
`docker compose up --build`

---

## Observações

- Os certificados são locais → o navegador exibirá aviso de segurança.

- A API irá criar automaticamente um usuário **Admin** com login **Admin** e senha **123456**.

## Bibliotecas Utilizadas

## Flutter (Dart)

```
 ├─ UI
 │   ├─ material / cupertino
 │
 ├─ Estado
 │   └─ provider
 │
 ├─ Rede
 │   └─ dio
 │
 ├─ Persistência local
 │   ├─ drift (ORM)
 │   ├─ sqlite3_flutter_libs
 │   ├─ path_provider
 │   └─ path
 │
 ├─ Segurança
 │   ├─ flutter_secure_storage
 │   └─ jwt_decoder
 │
 ├─ Recursos do dispositivo
 │   ├─ image_picker
 │   ├─ geolocator
 │   └─ geocoding
 │
 ├─ Sistema
 │   ├─ connectivity_plus
 │   └─ url_launcher
 │
 └─ Utilitários
     └─ uuid
```

---

## API Relatórios (.NET 10)

```
├─ Presentation
│   ├─ Swagger (Swashbuckle)
│   ├─ OpenAPI
│   └─ FluentValidation.AspNetCore
│
├─ Application
│   ├─ FluentValidation
│   ├─ Azure Blob Storage
│   ├─ SkiaSharp (imagens)
│   └─ OpenLocationCode (geolocalização)
│
├─ Infrastructure
│   ├─ Entity Framework Core
│   ├─ PostgreSQL (Npgsql)
│   ├─ JWT
│   └─ OpenXML (documentos)
│
├─ Cross-Cutting (IOC)
│   ├─ JWT Bearer
│   ├─ Options Pattern
│   └─ Dependency Injection
```

Creditar futuralmente a biblioteca FluentValidate
