using MediatR;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.GetDefectDetails
{
    public record GetDefectDetailsQuery(Guid DefectId) : IRequest<GetDefectDetailsResponse?>;
}
