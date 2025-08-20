using GestaoDefeitos.Application.UseCases.ContributorUseCases.GetAllContributors;
using GestaoDefeitos.Application.UseCases.ContributorUseCases.GetCurrentContributor;
using GestaoDefeitos.Application.UseCases.ContributorUseCases.Register;
using GestaoDefeitos.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GestaoDefeitos.WebApi.Endpoints
{
    public static class ContributorEndpoints
    {
        public static IEndpointRouteBuilder MapContributorEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("/contributors")
                .WithTags("Contributors");

            group.MapGetContributorEndpoint();
            group.MapCreateContributorEndpoint();
            group.MapLogoutContributorEndpoint();
            group.MapGetAllContributorsEndpoint();

            return endpoints;
        }

        public static RouteGroupBuilder MapGetContributorEndpoint(this RouteGroupBuilder group)
        {
            group.MapGet("/me", async (IMediator mediator) =>
            {
                var query = new GetCurrentContributorQuery();
                var user = await mediator.Send(query);
                return (user is not null) ? Results.Ok(user) : Results.NotFound("Contributor not found.");
            })
            .RequireAuthorization();

            return group;
        }

        public static RouteGroupBuilder MapLogoutContributorEndpoint(this RouteGroupBuilder group)
        {
            group.MapGet("/logout", async (SignInManager<Contributor> signInManager) =>
            {
                await signInManager.SignOutAsync();
                return Results.Ok();
            })
            .RequireAuthorization();

            return group;
        }

        public static RouteGroupBuilder MapCreateContributorEndpoint(this RouteGroupBuilder group)
        {
            group.MapPost("/register", async (
                RegisterContributorCommand command,
                IMediator mediator) =>
            {
                var id = await mediator.Send(command);

                return (id is not null) 
                    ? Results.Created($"/contributors/{id.ContributorId}", id)
                    : Results.BadRequest("Failed to create contributor.");
            
            });

            return group;
        }

        public static RouteGroupBuilder MapGetAllContributorsEndpoint(this RouteGroupBuilder group)
        {
            group.MapGet("/getAllContributors", async (IMediator mediator) =>
            {
                var query = new GetAllContributorsQuery();
                var result = await mediator.Send(query);
                return (result is not null) ? Results.Ok(result) : Results.NotFound("Failed to fetch contributors.");
            })
            .RequireAuthorization();

            return group;
        }
    }
}
