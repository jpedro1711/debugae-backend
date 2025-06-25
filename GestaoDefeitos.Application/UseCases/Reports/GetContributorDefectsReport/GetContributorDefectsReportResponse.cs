using GestaoDefeitos.Domain.ViewModels;

namespace GestaoDefeitos.Application.UseCases.Reports.GetContributorDefectsReport
{
    public record GetContributorDefectsReportResponse(List<DefectsSimplifiedViewModel> defects);
}
