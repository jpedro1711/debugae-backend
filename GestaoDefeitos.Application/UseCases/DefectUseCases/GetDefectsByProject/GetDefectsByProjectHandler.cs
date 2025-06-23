using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.GetDefectsByProject
{
    public class GetDefectsByProjectHandler(
        IDefectRepository defectRepository
        ) : IRequestHandler<GetDefectsByProjectQuery, GetDefectsByProjectResponse>
    {
        public async Task<GetDefectsByProjectResponse> Handle(GetDefectsByProjectQuery request, CancellationToken cancellationToken)
        {
            var pagedDefects = await defectRepository.GetDefectsByProjectPagedAsync(
            request.ProjectId, request.Page, request.PageSize, cancellationToken);

            return new GetDefectsByProjectResponse(pagedDefects);
        }
    }
}
