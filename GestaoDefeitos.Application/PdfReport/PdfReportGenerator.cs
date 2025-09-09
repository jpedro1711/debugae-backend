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
                        var statusGroups = defects
                            .GroupBy(d => d.Status.ToUpper())
                            .OrderBy(g => g.Key)
                            .ToDictionary(g => g.Key, g => g.Count());

                        var priorityGroups = defects
                            .GroupBy(d => d.DefectPriority.ToUpper())
                            .OrderBy(g => g.Key)
                            .ToDictionary(g => g.Key, g => g.Count());

                        stack.Item().Row(row =>
                        {
                            row.RelativeItem().Element(CardStyle).Column(card =>
                            {
                                card.Item().Text("Defeitos por Status").Bold().FontSize(12);
                                foreach (var item in statusGroups)
                                    card.Item().Text($"{item.Key}: {item.Value}");
                            });

                            row.ConstantItem(20); // Espaço entre os cards

                            row.RelativeItem().Element(CardStyle).Column(card =>
                            {
                                card.Item().Text("Defeitos por Prioridade").Bold().FontSize(12);
                                foreach (var item in priorityGroups)
                                    card.Item().Text($"{item.Key}: {item.Value}");
                            });

                            static IContainer CardStyle(IContainer container) =>
                                container.Border(1)
                                         .BorderColor(Colors.Grey.Lighten1)
                                         .Padding(10)
                                         .Background(Colors.Grey.Lighten4)
                                         .ShowOnce();
                        });

                        stack.Item().PaddingTop(20);

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
                                                 .PaddingBottom(4)
                                                 .PaddingTop(2);
                                });

                                int index = 1;
                                foreach (var defect in defects)
                                {
                                    table.Cell().Text(index.ToString());
                                    table.Cell().Text(defect.Summary);
                                    table.Cell().Text(defect.Status.ToUpper());
                                    table.Cell().AlignRight().Text(defect.DefectPriority.ToUpper());

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
                        });
                });
            });
        }
    }
}
