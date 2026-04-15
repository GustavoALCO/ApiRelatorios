from pathlib import Path
from docxtpl import DocxTemplate, InlineImage
from docx.shared import Cm
import requests
from io import BytesIO
import copy

BASE_DIR = Path(__file__).resolve().parent.parent
TEMPLATE_PATH = BASE_DIR / "Templates" / "template2.docx"


def baixar_imagem(url: str):
    try:
        print(f"[IMG] Baixando: {url}")

        response = requests.get(url, timeout=15)

        print(f"[IMG] Status: {response.status_code}")

        if response.status_code != 200:
            return None

        return BytesIO(response.content)

    except Exception as ex:
        print(f"[IMG] ERRO: {url} -> {ex}")
        return None


def gerar_docx_imagens(dados: dict) -> BytesIO:
    dados = copy.deepcopy(dados)

    doc = DocxTemplate(TEMPLATE_PATH)

    evidencias = dados.get("Evidencias", [])

    for evid in evidencias:

        imagens_urls = evid.get("images", [])
        imagens_convertidas = []

        print(f"\n\n[EVIDENCIA] {len(imagens_urls)} imagens\n\n")

        for index, img_url in enumerate(imagens_urls):

            img_bytes = baixar_imagem(img_url)

            if not img_bytes:
                print(f"[EVIDENCIA] Falha ao baixar imagem: {img_url}")
                continue

            # regra de tamanho simples e estável
            altura = Cm(9.8) if len(imagens_urls) <= 2 else Cm(4.8)

            imagens_convertidas.append(
                InlineImage(doc, img_bytes, height=altura)
            )

        # substitui URLs por imagens renderizadas
        evid["images"] = imagens_convertidas

    print("\n===== DEBUG EVIDENCIAS =====")

    for i, e in enumerate(dados["Evidencias"]):
        print(f"\n--- Evidencia {i} ---")
        print("CID:", e.get("CID"))
        print("ALI:", e.get("ALI"))
        print("DESC:", e.get("DESC"))
        print("IMAGENS COUNT:", len(e.get("images", [])))
        print("IMAGENS TYPE:", [type(x) for x in e.get("images", [])])

    print("\n===========================\n")

    doc.render(dados)

    file_stream = BytesIO()
    doc.save(file_stream)
    file_stream.seek(0)

    print("[DOCX] Finalizado")

    return file_stream