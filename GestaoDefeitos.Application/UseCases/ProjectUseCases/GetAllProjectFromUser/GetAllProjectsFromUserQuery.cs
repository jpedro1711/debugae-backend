using MediatR;

namespace GestaoDefeitos.Application.UseCases.ProjectUseCases.GetAllProjectFromUser
{
    public record GetAllProjectsFromUserQuery() : IRequest<GetAllProjectFromUserResponse>;
}
