using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.ProjectUseCases.UpdateProject
{
    public class UpdateProjectHandler(IProjectRepository projectRepository) : IRequestHandler<UpdateProjectRequest, UpdateProjectResponse>
    {
        public async Task<UpdateProjectResponse> Handle(UpdateProjectRequest request, CancellationToken cancellationToken)
        {
            var existingProject = await projectRepository.GetByIdAsync(request.ProjectId);

            if (existingProject == null)
            {
                throw new InvalidOperationException($"Project with ID {request.ProjectId} not found.");
            }

            existingProject.Name = request.ProjectName;
            existingProject.Description = request.ProjectDescription;

            await projectRepository.UpdateAsync(existingProject);

            return new UpdateProjectResponse(request.ProjectId, request.ProjectName, request.ProjectDescription);
        }
    }
}
