# Imagem base leve
FROM python:3.11-slim

# Evita criar arquivos .pyc e buffer no log
ENV PYTHONDONTWRITEBYTECODE=1
ENV PYTHONUNBUFFERED=1

# Diretório da aplicação
WORKDIR /app

# Copia apenas requirements primeiro (cache de build)
COPY requirements.txt .

# Instala dependências
RUN pip install --no-cache-dir -r requirements.txt

# Copia o restante do projeto
COPY . .

# Expõe a porta da API
EXPOSE 8000

# Comando para subir FastAPI
CMD ["uvicorn", "main:app", "--host", "0.0.0.0", "--port", "8000"]