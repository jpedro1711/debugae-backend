using MediatR;

namespace GestaoDefeitos.Application.UseCases.ProjectUseCases.GetProjectDetails
{
    public record GetProjectDetailsQuery(Guid ProjectId) : IRequest<GetProjectDetailsResponse?>;
}
