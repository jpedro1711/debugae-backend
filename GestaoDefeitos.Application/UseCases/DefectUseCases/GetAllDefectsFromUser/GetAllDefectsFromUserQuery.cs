using MediatR;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.GetAllDefectsFromUser
{
    public record GetAllDefectsFromUserQuery() : IRequest<GetAllDefectsFromUserResponse>;
}