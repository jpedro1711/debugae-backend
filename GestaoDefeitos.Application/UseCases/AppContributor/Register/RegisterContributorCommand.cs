using GestaoDefeitos.Domain.Enums;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.AppContributor.Register
{
    public record RegisterContributorCommand(
            string Firstname,
            string Lastname,
            string Email,
            string Password,
            string Department,
            ContributorProfession ContributorProfession
        ) : IRequest<RegisterContributorResponse?>;
}
