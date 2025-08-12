using MediatR;

namespace GestaoDefeitos.Application.UseCases.ProjectUseCases.UpdateProject
{
    public record UpdateProjectRequest(Guid ProjectId, string ProjectName, string ProjectDescription) : IRequest<UpdateProjectResponse>;
}
