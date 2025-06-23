using MediatR;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.GetUserDefects
{
    public record GetUserDefectsQuery(int Page = 1, int PageSize = 10) : IRequest<GetUserDefectsResponse>;
}
