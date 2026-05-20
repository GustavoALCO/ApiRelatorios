using APIRelatorios.Infra.Relatorios.Context;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;

namespace APIRelatorios.Infra.Relatorios.Componentes;

internal static class ImageCellComponentes
{
    private static uint _docId = 1;

    public static TableCell Criar(
        RelatorioContext ctx,
        byte[] imageBytes,
        long larguraEmus,
        long alturaEmus)
    {
        if (imageBytes == null || imageBytes.Length == 0)
            return new TableCell(new Paragraph());

        var imagePart = ctx.MainPart.AddImagePart(ImagePartType.Png);

        using (var stream = new MemoryStream(imageBytes))
            imagePart.FeedData(stream);

        var relId = ctx.MainPart.GetIdOfPart(imagePart);
        var id = _docId++;

        var drawing = new Drawing(
            new DW.Inline(
                new DW.Extent { Cx = larguraEmus, Cy = alturaEmus },
                new DW.EffectExtent
                {
                    LeftEdge = 0,
                    TopEdge = 0,
                    RightEdge = 0,
                    BottomEdge = 0
                },
                new DW.DocProperties
                {
                    Id = id,
                    Name = $"Imagem_{id}"
                },
                new DW.NonVisualGraphicFrameDrawingProperties(
                    new A.GraphicFrameLocks { NoChangeAspect = true }
                ),
                new A.Graphic(
                    new A.GraphicData(
                        new PIC.Picture(
                            new PIC.NonVisualPictureProperties(
                                new PIC.NonVisualDrawingProperties
                                {
                                    Id = 0,
                                    Name = $"Imagem_{id}"
                                },
                                new PIC.NonVisualPictureDrawingProperties()
                            ),
                            new PIC.BlipFill(
                                new A.Blip
                                {
                                    Embed = relId,
                                    CompressionState = A.BlipCompressionValues.Print
                                },
                                new A.Stretch(new A.FillRectangle())
                            ),
                            new PIC.ShapeProperties(
                                new A.Transform2D(
                                    new A.Offset { X = 0, Y = 0 },
                                    new A.Extents { Cx = larguraEmus, Cy = alturaEmus }

                                ),
                                new A.PresetGeometry(new A.AdjustValueList())
                                {
                                    
                                    Preset = A.ShapeTypeValues.Rectangle
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

        return new TableCell(
            new TableCellProperties(
                new TableCellVerticalAlignment
                {
                    Val = TableVerticalAlignmentValues.Center
                }
            ),
            new Paragraph(
                new ParagraphProperties(
                    new Justification
                    {
                        Val = JustificationValues.Center
                    }
                ),
                new Run(drawing)
            )
        );
    }
}