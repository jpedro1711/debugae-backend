using GestaoDefeitos.Domain.Enums;
using GestaoDefeitos.Domain.ViewModels;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;

namespace GestaoDefeitos.Application.PdfReport
{
    public static class PdfReportGenerator
    {
        public static IDocument GenerateUserDefectReport(List<DefectsSimplifiedViewModel>? defects, string loggedUsername, string? projectName = null)
        {
            if (defects is null)
                throw new ArgumentNullException(nameof(defects), "Defects cannot be null");

            var emissionDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm", new CultureInfo("pt-BR"));
            var userName = loggedUsername;

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Helvetica"));

                    // --- Header ---
                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(stack =>
                        {
                            stack.Item().Text("Debugaê")
                                .Bold()
                                .FontSize(20)
                                .FontColor(Colors.Grey.Darken4);

                            stack.Item().Text("Relatório de Defeitos")
                                .FontSize(12)
                                .FontColor(Colors.Grey.Darken2);

                            stack.Item().Text($"Emitido em: {emissionDate}")
                                .FontSize(10)
                                .FontColor(Colors.Grey.Darken1);

                            if (projectName is not null && projectName.Length > 0)
                            {
                                stack.Item().Text($"Projeto: {projectName}")
                                .FontSize(10)
                                .FontColor(Colors.Grey.Darken1);
                            }


                            stack.Item().Text($"Usuário: {userName}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken1);
                        });

                        row.ConstantItem(50);
                    });

                    page.Content().PaddingVertical(1, Unit.Centimetre).Column(stack =>
                    {
                        // Cálculos das Métricas
                        var statusGroups = defects
                            .GroupBy(d => d.Status.ToUpper())
                            .OrderBy(g => g.Key)
                            .ToDictionary(g => g.Key, g => g.Count());

                        var priorityGroups = defects
                            .GroupBy(d => d.DefectPriority.ToUpper())
                            .OrderBy(g => g.Key)
                            .ToDictionary(g => g.Key, g => g.Count());

                        var defectsBySeverity = defects
                            .GroupBy(d => d.DefectPriority)
                            .ToDictionary(g => g.Key, g => g.Count());

                        var defectByCategory = defects
                            .GroupBy(d => d.Category)
                            .ToDictionary(g => g.Key, g => g.Count());

                        var defectByVersion = defects
                            .GroupBy(d => d.Version)
                            .ToDictionary(g => g.Key, g => g.Count());

                        var resolvedDefects = defects
                            .Where(d => d.Status.Equals(DefectStatus.Resolved) && d.ResolvedAt.HasValue)
                            .ToList();

                        var mediumResolutionTimeInDays = resolvedDefects
                            .Select(d => (d.ResolvedAt.Value - d.CreatedAt).TotalDays)
                            .DefaultIfEmpty(0)
                            .Average();

                        var totalDefectsCount = defects.Count;

                        var resolutionIndex = (double)resolvedDefects.Count / totalDefectsCount;

                        static IContainer CardStyle(IContainer container) =>
                            container.Border(1)
                                     .BorderColor(Colors.Grey.Lighten1)
                                     .Padding(10)
                                     .Background(Colors.Grey.Lighten4)
                                     .ShowOnce();

                        stack.Item().Row(row =>
                        {
                            row.RelativeItem().Element(CardStyle).Column(card =>
                            {
                                card.Item().Text("Defeitos por Status").Bold().FontSize(12);
                                foreach (var item in statusGroups)
                                    card.Item().Text($"{item.Key}: {item.Value}");
                            });

                            row.ConstantItem(10);

                            row.RelativeItem().Element(CardStyle).Column(card =>
                            {
                                card.Item().Text("Defeitos por Prioridade").Bold().FontSize(12);
                                foreach (var item in priorityGroups)
                                    card.Item().Text($"{item.Key}: {item.Value}");
                            });
                        });

                        stack.Item().PaddingVertical(10);

                        stack.Item().Row(row =>
                        {
                            row.RelativeItem().Element(CardStyle).Column(card =>
                            {
                                card.Item().Text("Métricas Chave").Bold().FontSize(12);
                                card.Item().Text($"Total de Defeitos: {totalDefectsCount}");
                                card.Item().Text($"Tempo Médio de Resolução: {mediumResolutionTimeInDays:F2} dias");
                                card.Item().Text($"Índice de Resolução: {resolutionIndex:P2}");
                            });

                            row.ConstantItem(10); 

                            // Card por Categoria
                            row.RelativeItem().Element(CardStyle).Column(card =>
                            {
                                card.Item().Text("Defeitos por Categoria").Bold().FontSize(12);
                                foreach (var item in defectByCategory)
                                    card.Item().Text($"{item.Key}: {item.Value}");
                            });
                        });

                        stack.Item().PaddingTop(20);

                        stack.Item().Text("Lista de Defeitos").Bold().FontSize(14).Underline().ParagraphSpacing(5);

                        stack.Item().Element(container =>
                        {
                            container.Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(30);
                                    columns.RelativeColumn(3);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(2);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(HeaderStyle).Text("#").SemiBold();
                                    header.Cell().Element(HeaderStyle).Text("Sumário").SemiBold();
                                    header.Cell().Element(HeaderStyle).Text("Status").SemiBold();
                                    header.Cell().Element(HeaderStyle).AlignRight().Text("Prioridade").SemiBold();

                                    static IContainer HeaderStyle(IContainer container) =>
                                        container.Background(Colors.Grey.Lighten3)
                                                 .BorderBottom(1)
                                                 .BorderColor(Colors.Grey.Medium)
                                                 .PaddingVertical(4)
                                                 .PaddingHorizontal(5);
                                });

                                IContainer DataCellStyle(IContainer container, int index)
                                {
                                    var backgroundColor = index % 2 == 0 ? Colors.White : Colors.Grey.Lighten5;

                                    return container
                                        .Background(backgroundColor)
                                        .PaddingVertical(5)
                                        .PaddingHorizontal(5)
                                        .BorderBottom(1)
                                        .BorderColor(Colors.Grey.Lighten3);
                                }


                                int index = 1;
                                foreach (var defect in defects)
                                {
                                    table.Cell().Element(c => DataCellStyle(c, index)).Text(index.ToString());
                                    table.Cell().Element(c => DataCellStyle(c, index)).Text(defect.Summary);
                                    table.Cell().Element(c => DataCellStyle(c, index)).Text(defect.Status.ToUpper());
                                    table.Cell().Element(c => DataCellStyle(c, index)).AlignRight().Text(defect.DefectPriority.ToUpper());

                                    index++;
                                }
                            });
                        });
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Página ");
                            x.CurrentPageNumber();
                            x.Span(" de ");
                            x.TotalPages();
                        });
                });
            });
        }
    }
}