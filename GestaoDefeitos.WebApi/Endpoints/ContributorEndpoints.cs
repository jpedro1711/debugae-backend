using GestaoDefeitos.Application.Requests;
using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
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
                UserManager<Contributor> userManager,
                RegisterContributorRequest request) =>
            {
                var user = new Contributor
                {
                    Email = request.Email,
                    FullName = request.FullName,
                    UserName = request.Email,
                };

                var result = await userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                    return Results.BadRequest(result.Errors);

                return Results.Created($"/users/{user.Id}", new { user.Id, user.UserName, user.Email, user.FullName });
            });

            return group;
        }
    }
}
