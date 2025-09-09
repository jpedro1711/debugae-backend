using FuzzySharp;
using GestaoDefeitos.Domain.Interfaces.Repositories;
using GestaoDefeitos.Domain.ViewModels;
using MediatR;

namespace GestaoDefeitos.Application.UseCases.DefectUseCases.DetectDefectDuplicates
{
    public class DetectDefectDuplicatesHandler
        (
            IDefectRepository defectRepository
        ) : IRequestHandler<DetectDefectDuplicatesCommand, DetectDefectDuplicatesResponse>
    {
        public async Task<DetectDefectDuplicatesResponse> Handle(DetectDefectDuplicatesCommand command, CancellationToken cancellationToken)
        {
            var newDefect = new DefectDuplicatesViewModel
            {
                ProjectId = command.ProjectId.ToString(),
                Summary = command.Summary,
                Description = command.Description,
                Category = command.Category.ToString(),
                Severity = command.Severity.ToString(),
                Environment = command.Environment.ToString(),
                CreatedAt = DateTime.UtcNow,
                Status = command.Status.ToString(),
                Version = command.Version
            };

            var projectDefects = await defectRepository.GetDefectsDuplicatedViewModelByProjectAsync(new Guid(command.ProjectId), cancellationToken);

            var possibleDuplicates = Process.ExtractSorted(
                newDefect,
                projectDefects,
                d => $"{d.Summary} {d.Description}"
            );

            var duplicates = possibleDuplicates
                .Where(d => d.Score >= 60)
                .Select(d => new DefectDuplicatesViewModel
                {
                    DefectId = d.Value.DefectId,
                    ProjectId = d.Value.ProjectId,
                    Summary = d.Value.Summary,
                    Description = d.Value.Description,
                    Category = d.Value.Category,
                    Severity = d.Value.Severity,
                    Environment = d.Value.Environment,
                    Version = d.Value.Version,
                    CreatedAt = d.Value.CreatedAt,
                    Status = d.Value.Status,
                    Score = d.Score
                })
                .ToList();

            return new DetectDefectDuplicatesResponse(duplicates.Count, duplicates);
        }
    }
}
