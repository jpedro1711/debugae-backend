using GestaoDefeitos.Domain.Entities;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using GestaoDefeitos.Domain.ViewModels;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.GetDefectDetails
{
    public class GetDefectDetailsHandler
        (
            IDefectRepository defectRepository,
            IDefectHistoryRepository defectHistoryRepository
        ) : IRequestHandler<GetDefectDetailsQuery, GetDefectDetailsResponse?>
    {
        public async Task<GetDefectDetailsResponse?> Handle(GetDefectDetailsQuery query, CancellationToken cancellationToken)
        {
            var defectDetails = await defectRepository.GetDefectDetailsProjectionAsync(query.DefectId, cancellationToken);

            return CreateDefectDetailsResponse(defectDetails);
        }

        private static GetDefectDetailsResponse? CreateDefectDetailsResponse(DefectAllDetailsViewModel? defect)
        {

            return new GetDefectDetailsResponse(
                    defect?.DefectId,
                    defect?.DefectDescription,
                    defect?.DefectSummary,
                    defect?.CreatedAt,
                    defect?.CreatedByUser,
                    defect?.DefectSeverity,
                    defect?.DefectStatus,
                    defect?.ExpirationDate,
                    defect?.DefectCategory,
                    defect?.ResponsibleContributor,
                    defect?.Details,
                    defect?.Comments,
                    defect?.Attachment,
                    defect?.RelatedDefects
                );
        }
    }
}
