using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Pictures;
using DocumentFormat.OpenXml.Drawing.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace APIRelatorios.Infra.Relatorios.Componentes;

internal static class ImageCellComponentes
{
    public static DocumentFormat.OpenXml.Wordprocessing.TableCell Criar(
        MainDocumentPart mainPart,
        byte[] imageBytes,
        long larguraEmus,
        long alturaEmus,
        ImagePartType imageType = ImagePartType.Jpeg)
    {
        // 1️⃣ Cria ImagePart
        var imagePart = mainPart.AddImagePart(imageType);

        using (var stream = new MemoryStream(imageBytes))
        {
            imagePart.FeedData(stream);
        }

        var relId = mainPart.GetIdOfPart(imagePart);

        // 2️⃣ Cria Drawing
        var drawing = CriarDrawing(relId, larguraEmus, alturaEmus);

        // 3️⃣ Retorna célula pronta
        return new DocumentFormat.OpenXml.Wordprocessing.TableCell(
            new DocumentFormat.OpenXml.Wordprocessing.TableCellProperties(
                new TableCellVerticalAlignment
                {
                    Val = TableVerticalAlignmentValues.Center
                },
                new Shading
                {
                    Val = ShadingPatternValues.Clear,
                    Fill = "F8F8FF" // fundo claro
                }
            ),
            new DocumentFormat.OpenXml.Wordprocessing.Paragraph(
                new DocumentFormat.OpenXml.Wordprocessing.ParagraphProperties(
                    new Justification { Val = JustificationValues.Center }
                ),
                new DocumentFormat.OpenXml.Wordprocessing.Run(drawing)
            )
        );
    }

    private static Drawing CriarDrawing(string relId, long larguraEmus, long alturaEmus)
    {
        return new Drawing(
            new Inline(
                new Extent { Cx = larguraEmus, Cy = alturaEmus },
                new EffectExtent(),
                new DocProperties
                {
                    Id = 1U,
                    Name = "Imagem"
                },
                new Graphic(
                    new GraphicData(
                        new DocumentFormat.OpenXml.Drawing.Picture(
                            new DocumentFormat.OpenXml.Drawing.NonVisualPictureProperties(
                                new DocumentFormat.OpenXml.Drawing.NonVisualDrawingProperties
                                {
                                    Id = 0U,
                                    Name = "Imagem"
                                },
                                new DocumentFormat.OpenXml.Drawing.NonVisualPictureDrawingProperties()
                            ),
                            new DocumentFormat.OpenXml.Drawing.BlipFill(
                                new Blip
                                {
                                    Embed = relId,
                                    CompressionState = BlipCompressionValues.Print
                                },
                                new Stretch(new FillRectangle())
                            ),
                            new DocumentFormat.OpenXml.Drawing.ShapeProperties(
                                new Transform2D(
                                    new Offset { X = 0, Y = 0 },
                                    new Extents
                                    {
                                        Cx = larguraEmus,
                                        Cy = alturaEmus
                                    }
                                ),
                                new PresetGeometry(new AdjustValueList())
                                {
                                    Preset = ShapeTypeValues.Rectangle
                                }
                            )
                        )
                    )
                    {
                        Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture"
                    }
                )
            )
        );
    }
}
