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
        private static readonly string MainColor = Colors.Purple.Darken2;

        private static string MapStringToPortuguese(string? value, Type enumType)
        {
            if (string.IsNullOrWhiteSpace(value) || enumType is null)
                return "Desconhecido";

            try
            {
                if (System.Enum.TryParse(enumType, value, true, out object? enumValue) && enumValue is System.Enum e)
                    return e.GetPortugueseDescription();
            }
            catch
            {
                // ignore and fallback
            }
            return value;
        }

        public static IDocument GenerateUserDefectReport(List<DefectsSimplifiedViewModel>? defects, string loggedUsername, string? projectName = null)
        {
            if (defects is null)
                throw new ArgumentNullException(nameof(defects), "Defects cannot be null");

            var emissionDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm", new CultureInfo("pt-BR"));
            var userName = loggedUsername ?? string.Empty;

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Helvetica"));

                    // Header banner
                    page.Header().Element(header =>
                    {
                        header.Background(MainColor).Padding(14).Row(row =>
                        {
                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text("Debugaê").Bold().FontSize(22).FontColor(Colors.White);
                                col.Item().Text("Relatório de Defeitos").FontSize(12).FontColor(Colors.Grey.Lighten3);
                            });
                            // spacer removed to avoid conflicting padding on fixed width container
                            row.RelativeItem().AlignRight().Column(col =>
                            {
                                col.Item().Text(text =>
                                {
                                    text.Span("Emitido em: ").SemiBold().FontColor(Colors.White);
                                    text.Span(emissionDate).FontColor(Colors.White);
                                });
                            });
                        });
                    });

                    page.Content().PaddingVertical(1, Unit.Centimetre).Column(stack =>
                    {
                        // Chips for context (Project / User)
                        stack.Item().Row(row =>
                        {
                            if (!string.IsNullOrWhiteSpace(projectName))
                            {
                                row.AutoItem().Element(Chip).Text($"Projeto: {projectName}");
                                row.ConstantItem(8);
                            }
                            row.AutoItem().Element(Chip).Text($"Usuário: {userName}");
                        });

                        stack.Item().PaddingVertical(10);

                        // Metrics and group calculations
                        var statusGroups = defects
                            .GroupBy(d => MapStringToPortuguese(d.Status, typeof(DefectStatus)))
                            .OrderBy(g => g.Key)
                            .ToDictionary(g => g.Key, g => g.Count());

                        var priorityGroups = defects
                            .GroupBy(d => MapStringToPortuguese(d.DefectPriority, typeof(DefectPriority)))
                            .OrderBy(g => g.Key)
                            .ToDictionary(g => g.Key, g => g.Count());

                        var categoryGroups = defects
                            .GroupBy(d => MapStringToPortuguese(d.Category, typeof(DefectCategory)))
                            .OrderBy(g => g.Key)
                            .ToDictionary(g => g.Key, g => g.Count());

                        var versionGroups = defects
                            .GroupBy(d => string.IsNullOrWhiteSpace(d.Version) ? "Sem versão" : d.Version)
                            .OrderBy(g => g.Key)
                            .ToDictionary(g => g.Key, g => g.Count());

                        var resolvedDefects = defects
                            .Where(d => string.Equals(d.Status, DefectStatus.Resolved.ToString(), StringComparison.OrdinalIgnoreCase) && d.ResolvedAt.HasValue)
                            .ToList();

                        decimal mediumResolutionTimeInDays = (decimal)resolvedDefects
                            .Select(d => (d.ResolvedAt!.Value - d.CreatedAt).TotalDays)
                            .DefaultIfEmpty(0)
                            .Average();

                        var severityGroups = defects
                            .GroupBy(d => MapStringToPortuguese(d.Severity, typeof(DefectSeverity)))
                            .OrderBy(g => g.Key)
                            .ToDictionary(g => g.Key, g => g.Count());

                        var totalDefectsCount = defects.Count;

                        var resolutionIndex = Math.Round((double)resolvedDefects.Count / (totalDefectsCount > 0 ? totalDefectsCount : 1) * 100, 2);

                        var invalidDefectsIndex = Math.Round(
                            (double)defects.Count(d => string.Equals(d.Status, DefectStatus.Invalid.ToString(), StringComparison.OrdinalIgnoreCase))
                            / (totalDefectsCount > 0 ? totalDefectsCount : 1)
                            * 100,
                            2);

                        // KPI summary cards
                        stack.Item().Row(row =>
                        {
                            row.RelativeItem().Element(SummaryCard).Column(card =>
                            {
                                card.Item().Text("Total de Defeitos").FontColor(Colors.Grey.Darken2);
                                card.Item().Text(totalDefectsCount.ToString()).Bold().FontSize(20).FontColor(MainColor);
                            });
                            row.ConstantItem(10);
                            row.RelativeItem().Element(SummaryCard).Column(card =>
                            {
                                card.Item().Text("Tempo Médio de Resolução (dias)").FontColor(Colors.Grey.Darken2);
                                card.Item().Text($"{mediumResolutionTimeInDays:F2}").Bold().FontSize(20).FontColor(MainColor);
                            });
                            row.ConstantItem(10);
                            row.RelativeItem().Element(SummaryCard).Column(card =>
                            {
                                card.Item().Text("Índice de Resolução").FontColor(Colors.Grey.Darken2);
                                card.Item().Text($"{resolutionIndex}%").Bold().FontSize(20).FontColor(MainColor);
                            });
                            row.ConstantItem(10);
                            row.RelativeItem().Element(SummaryCard).Column(card =>
                            {
                                card.Item().Text("Índice de Inválidos").FontColor(Colors.Grey.Darken2);
                                card.Item().Text($"{invalidDefectsIndex}%").Bold().FontSize(20).FontColor(MainColor);
                            });
                        });

                        stack.Item().PaddingVertical(12);

                        // Distribution cards: Status / Prioridade
                        stack.Item().Row(row =>
                        {
                            row.RelativeItem().Element(Card).Column(card =>
                            {
                                CardHeader(card, "Defeitos por Status");
                                foreach (var item in statusGroups)
                                    card.Item().Text($"{item.Key}: {item.Value}");
                            });

                            row.ConstantItem(10);

                            row.RelativeItem().Element(Card).Column(card =>
                            {
                                CardHeader(card, "Defeitos por Prioridade");
                                foreach (var item in priorityGroups)
                                    card.Item().Text($"{item.Key}: {item.Value}");
                            });
                        });

                        stack.Item().PaddingVertical(10);

                        // Distribution cards: Categoria / Severidade
                        stack.Item().Row(row =>
                        {
                            row.RelativeItem().Element(Card).Column(card =>
                            {
                                CardHeader(card, "Defeitos por Categoria");
                                foreach (var item in categoryGroups)
                                    card.Item().Text($"{item.Key}: {item.Value}");
                            });

                            row.ConstantItem(10);

                            row.RelativeItem().Element(Card).Column(card =>
                            {
                                CardHeader(card, "Defeitos por Severidade");
                                foreach (var item in severityGroups)
                                    card.Item().Text($"{item.Key}: {item.Value}");
                            });
                        });

                        stack.Item().PaddingVertical(10);

                        // Versions
                        stack.Item().Element(Card).Column(card =>
                        {
                            CardHeader(card, "Defeitos por Versão");
                            // Display versions in two columns for better readability
                            card.Item().Row(row =>
                            {
                                var half = (int)Math.Ceiling(versionGroups.Count / 2.0);
                                var left = versionGroups.Take(half);
                                var right = versionGroups.Skip(half);

                                row.RelativeItem().Column(col =>
                                {
                                    foreach (var v in left)
                                        col.Item().Text($"{v.Key}: {v.Value}");
                                });
                                row.RelativeItem().Column(col =>
                                {
                                    foreach (var v in right)
                                        col.Item().Text($"{v.Key}: {v.Value}");
                                });
                            });
                        });

                        stack.Item().PaddingTop(16);
                        stack.Item().Text("Lista de Defeitos").Bold().FontSize(14).FontColor(MainColor).Underline().ParagraphSpacing(5);

                        // Table
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
                                    header.Cell().Element(HeaderStyle).Text("#").SemiBold().FontColor(Colors.White);
                                    header.Cell().Element(HeaderStyle).Text("Sumário").SemiBold().FontColor(Colors.White);
                                    header.Cell().Element(HeaderStyle).Text("Status").SemiBold().FontColor(Colors.White);
                                    header.Cell().Element(HeaderStyle).AlignRight().Text("Prioridade").SemiBold().FontColor(Colors.White);

                                    static IContainer HeaderStyle(IContainer container) =>
                                        container.Background(MainColor)
                                                 .BorderBottom(1)
                                                 .BorderColor(MainColor)
                                                 .PaddingVertical(6)
                                                 .PaddingHorizontal(6);
                                });

                                static IContainer DataCellStyle(IContainer container, int index)
                                {
                                    var backgroundColor = index % 2 == 0 ? Colors.White : Colors.Purple.Lighten5;

                                    return container
                                        .Background(backgroundColor)
                                        .PaddingVertical(5)
                                        .PaddingHorizontal(6)
                                        .BorderBottom(1)
                                        .BorderColor(Colors.Grey.Lighten3);
                                }

                                int index = 1;
                                foreach (var defect in defects)
                                {
                                    var statusPtBr = MapStringToPortuguese(defect.Status, typeof(DefectStatus));
                                    var priorityPtBr = MapStringToPortuguese(defect.DefectPriority, typeof(DefectPriority));

                                    table.Cell().Element(c => DataCellStyle(c, index)).Text(index.ToString());
                                    table.Cell().Element(c => DataCellStyle(c, index)).Text(defect.Summary ?? string.Empty);
                                    table.Cell().Element(c => DataCellStyle(c, index)).Text(statusPtBr);
                                    table.Cell().Element(c => DataCellStyle(c, index)).AlignRight().Text(priorityPtBr);

                                    index++;
                                }
                            });
                        });

                        // Local UI helpers
                        static void CardHeader(ColumnDescriptor card, string title)
                        {
                            card.Item().Row(r =>
                            {
                                r.ConstantItem(4).Background(MainColor); // width-only accent bar that matches row height
                                r.RelativeItem().PaddingLeft(6).AlignMiddle().Text(title).Bold().FontSize(12).FontColor(MainColor);
                            });
                            card.Item().PaddingBottom(4);
                        }

                        static IContainer Card(IContainer container) =>
                            container.Border(1)
                                     .BorderColor(Colors.Grey.Lighten1)
                                     .Padding(10)
                                     .Background(Colors.White);

                        static IContainer SummaryCard(IContainer container) =>
                            container.Border(1)
                                     .BorderColor(Colors.Grey.Lighten1)
                                     .Padding(12)
                                     .Background(Colors.Purple.Lighten5);

                        static IContainer Chip(IContainer container) =>
                            container.Background(Colors.Purple.Lighten5)
                                     .PaddingVertical(4)
                                     .PaddingHorizontal(8)
                                     .Border(1)
                                     .BorderColor(Colors.Purple.Lighten3);
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Página ").FontSize(9).FontColor(Colors.Grey.Medium);
                            x.CurrentPageNumber().FontSize(9).FontColor(Colors.Grey.Medium);
                            x.Span(" de ").FontSize(9).FontColor(Colors.Grey.Medium);
                            x.TotalPages().FontSize(9).FontColor(Colors.Grey.Medium);
                        });
                });
            });
        }
    }
}