from pathlib import Path
from docxtpl import DocxTemplate
from io import BytesIO

BASE_DIR = Path(__file__).resolve().parent.parent
TEMPLATE_PATH = BASE_DIR / "Templates" / "template.docx"


def gerar_docx_emergencial(dados: dict) -> BytesIO:
    doc = DocxTemplate(TEMPLATE_PATH)

    doc.render(dados)

    file_stream = BytesIO()
    doc.save(file_stream)
    file_stream.seek(0)

    return file_stream