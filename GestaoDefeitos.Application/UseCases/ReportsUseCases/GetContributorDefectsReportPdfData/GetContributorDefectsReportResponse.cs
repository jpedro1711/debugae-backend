using GestaoDefeitos.Domain.ViewModels;

namespace GestaoDefeitos.Application.UseCases.Reports.GetContributorDefectsReportPdfData
{
    public record GetContributorDefectsReportResponse(List<DefectsSimplifiedViewModel> defects);
}
