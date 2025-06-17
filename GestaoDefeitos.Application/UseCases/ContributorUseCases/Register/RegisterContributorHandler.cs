using GestaoDefeitos.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GestaoDefeitos.Application.UseCases.ContributorUseCases.Register
{
    public class RegisterContributorHandler(UserManager<Contributor> userManager) : IRequestHandler<RegisterContributorCommand, RegisterContributorResponse?>
    {
        public async Task<RegisterContributorResponse?> Handle(RegisterContributorCommand command, CancellationToken cancellationToken)
        {
            Contributor contributor = new()
            {
                Firstname = command.Firstname,
                Lastname = command.Lastname,
                Email = command.Email,
                UserName = command.Email,
                Department = command.Department,
                ContributorProfession = command.ContributorProfession
            };

            var createdContributor = await userManager.CreateAsync(contributor, command.Password);

            if (!createdContributor.Succeeded)
                return null;

            return new RegisterContributorResponse(contributor.Id);
        }
    }
}
