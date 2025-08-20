using GestaoDefeitos.Domain.ViewModels;

namespace GestaoDefeitos.Application.UseCases.ContributorUseCases.GetAllContributors
{
    public record GetAllContributorsResponse(List<ColaboratorViewModel> Colaborators);
}
