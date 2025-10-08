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

        private static string MapStringToPortuguese(string value, Type enumType)
        {
            if (System.Enum.TryParse(enumType, value, true, out object enumValue))
            {
                return ((System.Enum)enumValue).GetPortugueseDescription();
            }
            return value;
        }

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
                                .FontSize(24)
                                .FontColor(MainColor); // Cor roxa para o título

                            stack.Item().Text("Relatório de Defeitos")
                                .FontSize(14)
                                .FontColor(Colors.Grey.Darken2);

                            stack.Item().PaddingTop(5).Text(text =>
                            {
                                text.Span("Emitido em: ").SemiBold().FontSize(10);
                                text.Span(emissionDate).FontSize(10);
                            });

                            if (projectName is not null && projectName.Length > 0)
                            {
                                stack.Item().Text(text =>
                                {
                                    text.Span("Projeto: ").SemiBold().FontSize(10);
                                    text.Span(projectName).FontSize(10);
                                });
                            }


                            stack.Item().Text(text =>
                            {
                                text.Span("Usuário: ").SemiBold().FontSize(10);
                                text.Span(userName).FontSize(10);
                            });
                        });

                        row.ConstantItem(50);
                    });

                    // --- Content ---
                    page.Content().PaddingVertical(1, Unit.Centimetre).Column(stack =>
                    {
                        // Cálculos das Métricas e Mapeamento para PT-BR
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

                        // Variável defectByVersion adicionada ao relatório
                        var versionGroups = defects
                            .GroupBy(d => d.Version)
                            .OrderBy(g => g.Key)
                            .ToDictionary(g => g.Key, g => g.Count());

                        var resolvedDefects = defects
                            .Where(d => d.Status.Equals(DefectStatus.Resolved.ToString()) && d.ResolvedAt.HasValue)
                            .ToList();

                        decimal mediumResolutionTimeInDays = (decimal)resolvedDefects
                            .Select(d => (d.ResolvedAt.Value - d.CreatedAt).TotalDays)
                            .DefaultIfEmpty(0)
                            .Average();

                        var severityGroups = defects
                            .GroupBy(d => MapStringToPortuguese(d.Severity, typeof(DefectSeverity)))
                            .OrderBy(g => g.Key)
                            .ToDictionary(g => g.Key, g => g.Count());

                        var totalDefectsCount = defects.Count;

                        var resolutionIndex = Math.Round((double)resolvedDefects.Count / (totalDefectsCount > 0 ? totalDefectsCount : 1) * 100, 2);

                        // Estilo de Card Aprimorado
                        static QuestPDF.Infrastructure.IContainer CardStyle(QuestPDF.Infrastructure.IContainer container) =>
                            container.Border(1)
                                     .BorderColor(Colors.Grey.Lighten1)
                                     .Padding(10)
                                     .Background(Colors.Grey.Lighten5); // Fundo mais claro

                        // Card de Defeitos por Status e Prioridade
                        stack.Item().Row(row =>
                        {
                            row.RelativeItem().Element(CardStyle).Column(card =>
                            {
                                card.Item().Text("Defeitos por Status").Bold().FontSize(12).FontColor(MainColor).Underline();
                                foreach (var item in statusGroups)
                                    card.Item().Text($"{item.Key}: {item.Value}");
                            });

                            row.ConstantItem(10);

                            row.RelativeItem().Element(CardStyle).Column(card =>
                            {
                                card.Item().Text("Defeitos por Prioridade").Bold().FontSize(12).FontColor(MainColor).Underline();
                                foreach (var item in priorityGroups)
                                    card.Item().Text($"{item.Key}: {item.Value}");
                            });
                        });

                        stack.Item().PaddingVertical(10);

                        // Card de Métricas Chave e Categoria
                        stack.Item().Row(row =>
                        {
                            row.RelativeItem().Element(CardStyle).Column(card =>
                            {
                                card.Item().Text("Métricas Chave").Bold().FontSize(12).FontColor(MainColor).Underline();
                                card.Item().Text($"Total de Defeitos: {totalDefectsCount}");
                                card.Item().Text($"Tempo Médio de Resolução: {mediumResolutionTimeInDays:F2} dias");
                                card.Item().Text($"Índice de Resolução: {resolutionIndex}%");
                            });

                            row.ConstantItem(10);

                            // Card por Categoria
                            row.RelativeItem().Element(CardStyle).Column(card =>
                            {
                                card.Item().Text("Defeitos por Categoria").Bold().FontSize(12).FontColor(MainColor).Underline();
                                foreach (var item in categoryGroups)
                                    card.Item().Text($"{item.Key}: {item.Value}");
                            });
                        });

                        stack.Item().PaddingVertical(10);

                        // NOVO: Card de Defeitos por Severidade e Versão
                        stack.Item().Row(row =>
                        {
                            row.RelativeItem().Element(CardStyle).Column(card =>
                            {
                                card.Item().Text("Defeitos por Severidade").Bold().FontSize(12).FontColor(MainColor).Underline();
                                foreach (var item in severityGroups)
                                    card.Item().Text($"{item.Key}: {item.Value}");
                            });

                            // Card por Versão
                            row.RelativeItem().Element(CardStyle).Column(card =>
                            {
                                card.Item().Text("Defeitos por Versão").Bold().FontSize(12).FontColor(MainColor).Underline();
                                foreach (var item in versionGroups)
                                    card.Item().Text($"{item.Key}: {item.Value}");
                            });
                        });


                        stack.Item().PaddingTop(20);

                        stack.Item().Text("Lista de Defeitos").Bold().FontSize(14).FontColor(MainColor).Underline().ParagraphSpacing(5);

                        // --- Tabela de Defeitos ---
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

                                // Estilo do Cabeçalho da Tabela
                                table.Header(header =>
                                {
                                    header.Cell().Element(HeaderStyle).Text("#").SemiBold();
                                    header.Cell().Element(HeaderStyle).Text("Sumário").SemiBold();
                                    header.Cell().Element(HeaderStyle).Text("Status").SemiBold();
                                    header.Cell().Element(HeaderStyle).AlignRight().Text("Prioridade").SemiBold();

                                    static QuestPDF.Infrastructure.IContainer HeaderStyle(QuestPDF.Infrastructure.IContainer container) =>
                                        container.Background(MainColor) // Cor roxa para o cabeçalho
                                                 .BorderBottom(1)
                                                 .BorderColor(MainColor)
                                                 .PaddingVertical(4)
                                                 .PaddingHorizontal(5);
                                });

                                // Estilo das Células de Dados (linhas zebradas)
                                QuestPDF.Infrastructure.IContainer DataCellStyle(QuestPDF.Infrastructure.IContainer container, int index)
                                {
                                    var backgroundColor = index % 2 == 0 ? Colors.White : Colors.Purple.Lighten5; // Linhas zebradas com roxo claro

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
                                    // Mapeamento em PT-BR na lista de defeitos
                                    var statusPtBr = MapStringToPortuguese(defect.Status, typeof(DefectStatus));
                                    var priorityPtBr = MapStringToPortuguese(defect.DefectPriority, typeof(DefectPriority));

                                    table.Cell().Element(c => DataCellStyle(c, index)).Text(index.ToString());
                                    table.Cell().Element(c => DataCellStyle(c, index)).Text(defect.Summary);
                                    table.Cell().Element(c => DataCellStyle(c, index)).Text(statusPtBr);
                                    table.Cell().Element(c => DataCellStyle(c, index)).AlignRight().Text(priorityPtBr);

                                    index++;
                                }
                            });
                        });
                    });

                    // --- Footer ---
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