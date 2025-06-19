using GestaoDefeitos.Application.UseCases.DefectUseCases.AddOrRemoveTag;
using GestaoDefeitos.Application.UseCases.DefectUseCases.CreateDefect;
using GestaoDefeitos.Application.UseCases.DefectUseCases.DetectDefectDuplicates;
using GestaoDefeitos.Application.UseCases.DefectUseCases.GetDefectsByProject;
using GestaoDefeitos.Application.UseCases.DefectUseCases.GetUserDefects;
using GestaoDefeitos.Application.UseCases.DefectUseCases.UpdateDefectStatus;
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
            group.MapCreateOrRemoveDefectTag();
            group.MapDetectDefectDuplicates();
            group.MapUpdateDefectStatus();

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

        public static RouteGroupBuilder MapCreateOrRemoveDefectTag(this RouteGroupBuilder group)
        {
            group.MapPost("/tags", async (
                AddOrRemoveTagCommand command,
                IMediator mediator) =>
            {
                var tagResponse = await mediator.Send(command);

                return (tagResponse is not null)
                    ? Results.Ok(tagResponse)
                    : Results.BadRequest("Failed to create or remove defect tag");

            }).RequireAuthorization();

            return group;
        }

        public static RouteGroupBuilder MapDetectDefectDuplicates(this RouteGroupBuilder group)
        {
            group.MapPost("/findDuplicates", async (
                DetectDefectDuplicatesCommand command,
                IMediator mediator) =>
            {
                var duplicates = await mediator.Send(command);

                return (duplicates is not null)
                    ? Results.Ok(duplicates)
                    : Results.BadRequest("Failed to find duplicates");

            }).RequireAuthorization();

            return group;
        }

        public static RouteGroupBuilder MapUpdateDefectStatus(this RouteGroupBuilder group)
        {
            group.MapPatch("/updateStatus", async (
                UpdateDefectStatusCommand command,
                IMediator mediator) =>
            {
                var updated = await mediator.Send(command);

                return (updated is not null)
                    ? Results.Ok(updated)
                    : Results.BadRequest("Failed to update defect status");

            }).RequireAuthorization();

            return group;
        }

    }
}
