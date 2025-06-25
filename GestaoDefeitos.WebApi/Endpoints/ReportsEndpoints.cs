using GestaoDefeitos.Application.UseCases.Reports.UserDefectsReport;
using MediatR;

namespace GestaoDefeitos.WebApi.Endpoints
{
    public static class ReportsEndpoints
    {
        public static IEndpointRouteBuilder MapReportEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("/reports")
                .WithTags("Reports");

            group.MapGetUserDefectsReport();
            group.MapDownloadPdfReport();

            return endpoints;
        }

        public static RouteGroupBuilder MapGetUserDefectsReport(this RouteGroupBuilder group)
        {
            group.MapPost("/getCurrentUserProjects", async (
                IMediator mediator) =>
            {
                var userDefectsReport = await mediator.Send(new UserDefectsReportQuery());

                return (userDefectsReport is not null)
                    ? Results.Ok(userDefectsReport)
                    : Results.BadRequest("Failed to fetch defect reports.");

            }).RequireAuthorization();

            return group;
        }

        public static RouteGroupBuilder MapDownloadPdfReport(this RouteGroupBuilder group)
        {
            group.MapGet("/downloadPdfReport", async (
                IMediator mediator) =>
            {
                //var userDefectsReport = await mediator.Send(new UserDefectsReportQuery());
                //var document = new UserDefectsReportDocument(userDefectsReport);
                //var pdfBytes = document.GeneratePdf();

                //return (pdfBytes is not null)
                //    ? Results.File(pdfBytes, "application/pdf", "relatorio-defeitos.pdf")
                //    : Results.Problem("Error exporting PDF.");

            }).RequireAuthorization();

            return group;
        }
    }
}
