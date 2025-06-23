using MediatR;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.GetDefectsByProject
{
    public record GetDefectsByProjectQuery(Guid ProjectId, int Page = 1, int PageSize = 10)
        : IRequest<GetDefectsByProjectResponse>;
}
