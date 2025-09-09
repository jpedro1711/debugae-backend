using GestaoDefeitos.Application.Utils;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.Reports.GetContributorDefectsReportPdfData
{
    public class GetContributorDefectsReportHandler(
        IDefectRepository defectRepository,
        AuthenticationContextAcessor authenticationContextAcessor
        ) : IRequestHandler<GetContributorDefectsReportRequest, GetContributorDefectsReportResponse?>
    {
        public async Task<GetContributorDefectsReportResponse?> Handle(
            GetContributorDefectsReportRequest request,
            CancellationToken cancellationToken)
        {
            var defects = await defectRepository.GetDefectsByContributorAsync(authenticationContextAcessor.GetCurrentLoggedUserId(), cancellationToken);
            return new GetContributorDefectsReportResponse(defects);
        }
    }
}
