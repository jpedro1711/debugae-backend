using GestaoDefeitos.Application.UseCases.DefectUseCases.CreateDefect;
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
    }
}
