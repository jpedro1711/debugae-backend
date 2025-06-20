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
    }
}
