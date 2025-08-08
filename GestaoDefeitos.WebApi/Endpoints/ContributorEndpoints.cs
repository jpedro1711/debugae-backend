using GestaoDefeitos.Application.UseCases.ContributorUseCases.GetCurrentContributor;
using GestaoDefeitos.Application.UseCases.ContributorUseCases.Register;
using GestaoDefeitos.Infrastructure.Database;
using MediatR;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
    }
}
