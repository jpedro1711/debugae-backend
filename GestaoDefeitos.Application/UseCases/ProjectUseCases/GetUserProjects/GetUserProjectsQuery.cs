using MediatR;

namespace GestaoDefeitos.Application.UseCases.ProjectUseCases.GetUserProjects
{
    public record GetUserProjectsQuery(int Page = 1, int PageSize = 10) : IRequest<GetUserProjectsResponse>
    {
    }
}
