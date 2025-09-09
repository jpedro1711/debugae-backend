using GestaoDefeitos.Application.UseCases.ProjectUseCases.CreateProject;
using GestaoDefeitos.Application.UseCases.ProjectUseCases.GetAllProjectFromUser;
using GestaoDefeitos.Application.UseCases.ProjectUseCases.GetProjectDetails;
using GestaoDefeitos.Application.UseCases.ProjectUseCases.GetUserProjects;
using GestaoDefeitos.Application.UseCases.ProjectUseCases.ManageContributors;
using GestaoDefeitos.Application.UseCases.ProjectUseCases.UpdateProject;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
            group.MapGetProjectDetails();
            group.MapManageContributorstEndpoint();
            group.MapGetAllUserProjects();
            group.MapUpdateProject();

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

        public static RouteGroupBuilder MapManageContributorstEndpoint(this RouteGroupBuilder group)
        {
            group.MapPost("/manageContributors", async (
                ManageContributorCommand command,
                IMediator mediator) =>
            {
                var response = await mediator.Send(command);

                return (response is not null)
                    ? Results.Ok(response)
                    : Results.BadRequest("Failed to manage contributors.");

            }).RequireAuthorization();

            return group;
        }

        public static RouteGroupBuilder MapGetUserProjects(this RouteGroupBuilder group)
        {
            group.MapGet("/getCurrentUserProjects", async (
                    IMediator mediator,
                    [FromQuery] int page = 1,
                    [FromQuery] int pageSize = 10
                ) =>
            {
                var userProjects = await mediator.Send(new GetUserProjectsQuery(page, pageSize));

                return (userProjects is not null)
                    ? Results.Ok(userProjects)
                    : Results.BadRequest("Failed to fetch user projects.");

            }).RequireAuthorization();

            return group;
        }

        public static RouteGroupBuilder MapGetProjectDetails(this RouteGroupBuilder group)
        {
            group.MapGet("/projectDetails", async (
                [FromQuery] Guid projectId,
                IMediator mediator) =>
            {
                var projectDetails = await mediator.Send(new GetProjectDetailsQuery(projectId));

                return (projectDetails is not null)
                    ? Results.Ok(projectDetails)
                    : Results.NotFound("Project not found.");

            }).RequireAuthorization();

            return group;
        }

        public static RouteGroupBuilder MapGetAllUserProjects(this RouteGroupBuilder group)
        {
            group.MapGet("/getAllProjectsFromUser", async (
                IMediator mediator) =>
            {
                var userProjects = await mediator.Send(new GetAllProjectsFromUserQuery());

                return (userProjects is not null)
                    ? Results.Ok(userProjects)
                    : Results.BadRequest("Failed to fetch user projects.");

            }).RequireAuthorization();

            return group;
        }

        public static RouteGroupBuilder MapUpdateProject(this RouteGroupBuilder group)
        {
            group.MapPatch("/updateProject", async (
                [FromBody] UpdateProjectRequest request,
                IMediator mediator) =>
            {
                var updatedProject = await mediator.Send(request);
                return (updatedProject is not null)
                    ? Results.Ok(updatedProject)
                    : Results.BadRequest("Failed to update project.");
            }).RequireAuthorization();

            return group;
        }
    }
}
