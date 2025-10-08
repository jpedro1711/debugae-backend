using GestaoDefeitos.Application.PdfReport;
using GestaoDefeitos.Application.UseCases.Reports.GetContributorDefectsReportPdfData;
using GestaoDefeitos.Application.UseCases.Reports.GetProjectDefectsReportPdfData;
using GestaoDefeitos.Application.UseCases.Reports.ProjectDefectReport;
using GestaoDefeitos.Application.UseCases.Reports.UserDefectsReport;
using GestaoDefeitos.Application.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;

namespace GestaoDefeitos.WebApi.Endpoints
{
    public static class ReportsEndpoints
    {
        public static IEndpointRouteBuilder MapReportEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("/reports")
                .WithTags("Reports");

            group.MapGetUserDefectsReport();
            group.MapDownloadUserPdfReport();
            group.MapDownloadProjectPdfReport();
            group.MapGetProjectDefectsReport();

            return endpoints;
        }

        public static RouteGroupBuilder MapGetUserDefectsReport(this RouteGroupBuilder group)
        {
            group.MapPost("/getCurrentUserDefectsReport", async (
                IMediator mediator) =>
            {
                var userDefectsReport = await mediator.Send(new UserDefectsReportQuery());

                return (userDefectsReport is not null)
                    ? Results.Ok(userDefectsReport)
                    : Results.BadRequest("Failed to fetch defect reports.");

            }).RequireAuthorization();

            return group;
        }

        public static RouteGroupBuilder MapGetProjectDefectsReport(this RouteGroupBuilder group)
        {
            group.MapPost("/getProjectDefectsReport", async (
                IMediator mediator, [FromQuery] Guid ProjectId, [FromQuery] DateTime? InitialDate, [FromQuery] DateTime? FinalDate) =>
            {
                var projectDefectsData = await mediator.Send(new ProjectDefectsReportQuery(ProjectId, InitialDate, FinalDate));

                return (projectDefectsData is not null)
                    ? Results.Ok(projectDefectsData)
                    : Results.BadRequest("Failed to fetch project reports.");

            }).RequireAuthorization();

            return group;
        }

        public static RouteGroupBuilder MapDownloadUserPdfReport(this RouteGroupBuilder group)
        {
            group.MapGet("/downloadUserPdfReport", async (
                AuthenticationContextAcessor authenticationAcessor,
                IMediator mediator) =>
            {
                var currentUserDefects = await mediator.Send(new GetContributorDefectsReportRequest());
                var document = PdfReportGenerator.GenerateUserDefectReport(currentUserDefects.defects, authenticationAcessor.GetCurrentLoggedUserName());
                var pdfBytes = document.GeneratePdf();

                return (pdfBytes is not null)
                    ? Results.File(pdfBytes, "application/pdf", "relatorio-defeitos.pdf")
                    : Results.Problem("Error exporting PDF.");

            }).RequireAuthorization();

            return group;
        }

        public static RouteGroupBuilder MapDownloadProjectPdfReport(this RouteGroupBuilder group)
        {
            group.MapGet("/downloadProjectPdfReport", async (
                AuthenticationContextAcessor authenticationAcessor,
                [FromQuery] Guid projectId,
                IMediator mediator) =>
            {
                var currentProjectDefects = await mediator.Send(new GetProjectDefectsReportRequest(projectId));
                var document = PdfReportGenerator.GenerateUserDefectReport(currentProjectDefects.Defects, authenticationAcessor.GetCurrentLoggedUserName(), currentProjectDefects.ProjectName);
                var pdfBytes = document.GeneratePdf();

                return (pdfBytes is not null)
                    ? Results.File(pdfBytes, "application/pdf", "relatorio-defeitos.pdf")
                    : Results.Problem("Error exporting PDF.");

            }).RequireAuthorization();

            return group;
        }
    }
}
