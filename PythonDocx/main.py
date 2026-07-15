from fastapi import FastAPI
from fastapi.responses import StreamingResponse
from Services.gerarOficioImagens import gerar_docx_imagens
from Services.gerarOficio import gerar_docx_emergencial

from io import BytesIO
import zipfile

app = FastAPI()

@app.get("/health")
async def health():
    return {
        "status": "healthy"
    }

@app.post("/gerar-doc")
def gerar_documento(dados: dict):
    # chama as funções para gerar os documentos em memória
    doc1 = gerar_docx_imagens(dados)
    doc2 = gerar_docx_emergencial(dados)

    # cria zip em memória
    zip_buffer = BytesIO()

    with zipfile.ZipFile(zip_buffer, "w") as zip_file:
        zip_file.writestr("relatorio_imagens.docx", doc1.getvalue())
        zip_file.writestr("relatorio_emergencial.docx", doc2.getvalue())

    # volta para o início do buffer para leitura
    zip_buffer.seek(0)

    # retorna como download os templates preenchidos
    return StreamingResponse(
        zip_buffer,
        media_type="application/zip",
        headers={
            "Content-Disposition": "attachment; filename=relatorios.zip"
        }
    )