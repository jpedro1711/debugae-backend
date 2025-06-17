using GestaoDefeitos.Application.UseCases.AppContributor.Register;
using GestaoDefeitos.Infrastructure.Database;
using MediatR;
using System.Security.Claims;

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
            group.MapGet("/me", async (ClaimsPrincipal claims, AppDbContext context) =>
            {
                var userId = Guid.Parse(claims.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
                return await context.Users.FindAsync(userId);
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
