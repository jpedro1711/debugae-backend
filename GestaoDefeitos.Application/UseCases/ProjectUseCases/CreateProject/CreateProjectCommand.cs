using MediatR;

namespace GestaoDefeitos.Application.UseCases.ProjectUseCases.CreateProject
{
    public record CreateProjectCommand(
        string ProjectName,
        string ProjectDescription,
        List<string> ContributorsEmails
        ) : IRequest<CreateProjectResponse?>;
}
