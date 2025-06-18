using MediatR;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.GetUserDefects
{
    public record GetUserDefectsQuery() : IRequest<GetUserDefectsResponse>;
}
