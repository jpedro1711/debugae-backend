using GestaoDefeitos.Application.UseCases.ProjectUseCases.CreateProject;
using GestaoDefeitos.Application.UseCases.ProjectUseCases.GetUserProjects;
using MediatR;

namespace GestaoDefeitos.WebApi.Endpoints
{
    public static class ProjectEndpoints
    {
        public static IEndpointRouteBuilder MapProjectEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("/projects")
                .WithTags("Projects");

            group.MapCreateProjectEndpoint(); 
            group.MapGetUserProjects();

            return endpoints;
        }

        public static RouteGroupBuilder MapCreateProjectEndpoint(this RouteGroupBuilder group)
        {
            group.MapPost("/create", async (
                CreateProjectCommand command,
                IMediator mediator) =>
            {
                var projectId = await mediator.Send(command);

                return (projectId is not null)
                    ? Results.Created($"/projects/{projectId}", projectId)
                    : Results.BadRequest("Failed to create project.");

            }).RequireAuthorization();

            return group;
        }

        public static RouteGroupBuilder MapGetUserProjects(this RouteGroupBuilder group)
        {
            group.MapPost("/getCurrentUserProjects", async (
                IMediator mediator) =>
            {
                var userProjects = await mediator.Send(new GetUserProjectsQuery());

                return (userProjects is not null)
                    ? Results.Ok(userProjects)
                    : Results.BadRequest("Failed to fetch user projects.");

            }).RequireAuthorization();

            return group;
        }
    }
}
