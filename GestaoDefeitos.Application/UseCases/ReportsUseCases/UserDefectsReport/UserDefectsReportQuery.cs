using MediatR;

namespace GestaoDefeitos.Application.UseCases.Reports.UserDefectsReport
{
    public record UserDefectsReportQuery() : IRequest<UserDefectsReportResponse?>;
}
