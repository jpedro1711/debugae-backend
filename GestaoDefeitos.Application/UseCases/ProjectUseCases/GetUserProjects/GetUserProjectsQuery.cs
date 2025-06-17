using MediatR;

namespace GestaoDefeitos.Application.UseCases.ProjectUseCases.GetUserProjects
{
    public record GetUserProjectsQuery() : IRequest<GetUserProjectsResponse>
    {
    }
}
