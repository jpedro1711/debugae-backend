using MediatR;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.GetDefectsByProject
{
    public record GetDefectsByProjectQuery(Guid ProjectId) : IRequest<GetDefectsByProjectResponse>;
}
