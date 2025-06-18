using GestaoDefeitos.Application.UseCases.DefectUseCases.CreateDefect;
using GestaoDefeitos.Application.UseCases.DefectUseCases.GetDefectsByProject;
using GestaoDefeitos.Application.UseCases.DefectUseCases.GetUserDefects;
using GestaoDefeitos.Application.UseCases.ProjectUseCases.GetUserProjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDefeitos.WebApi.Endpoints
{
    public static class DefectEndpoints
    {
        public static IEndpointRouteBuilder MapDefectEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("/defects")
                .WithTags("Defects");

            group.MapCreateDefectEndpoint();
            group.MapGetProjectDefects();
            group.MapGetUserDefects();

            return endpoints;
        }

        public static RouteGroupBuilder MapCreateDefectEndpoint(this RouteGroupBuilder group)
        {
            group.MapPost("/create", async (
                [FromForm] CreateDefectCommand command,
                IMediator mediator) =>
                {
                    var id = await mediator.Send(command);

                    return (id is not null)
                        ? Results.Created($"/defects/{id.DefectId}", id)
                        : Results.BadRequest("Failed to create defect.");
                })
            .RequireAuthorization()
            .DisableAntiforgery();

            return group;
        }

        public static RouteGroupBuilder MapGetProjectDefects(this RouteGroupBuilder group)
        {
            group.MapGet("/getProjectDefects", async (
                [FromQuery] Guid projectId,
                IMediator mediator) =>
            {
                GetDefectsByProjectQuery query = new GetDefectsByProjectQuery(projectId);
                var defects = await mediator.Send(query);

                return (defects is not null)
                    ? Results.Ok(defects)
                    : Results.BadRequest("Failed to fetch project defects.");

            }).RequireAuthorization();

            return group;
        }

        public static RouteGroupBuilder MapGetUserDefects(this RouteGroupBuilder group)
        {
            group.MapGet("/getCurrentUserDefects", async (
                IMediator mediator) =>
            {
                var userDefects = await mediator.Send(new GetUserDefectsQuery());

                return (userDefects is not null)
                    ? Results.Ok(userDefects)
                    : Results.BadRequest("Failed to fetch user defects.");

            }).RequireAuthorization();

            return group;
        }

    }
}
