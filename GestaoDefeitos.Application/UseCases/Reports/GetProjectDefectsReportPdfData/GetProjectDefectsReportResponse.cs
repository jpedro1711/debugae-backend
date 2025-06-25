using GestaoDefeitos.Domain.ViewModels;

namespace GestaoDefeitos.Application.UseCases.Reports.GetProjectDefectsReport
{
    public record GetProjectDefectsReportResponse(List<DefectsSimplifiedViewModel> Defects, string ProjectName);
}
