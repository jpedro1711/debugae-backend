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
                AssignedToUserId = command.AssignedToUserId,
                Summary = command.Summary,
                Description = command.Description,
                Category = command.Category.ToString(),
                Severity = command.Severity.ToString(),
                Environment = command.Environment.ToString(),
                Version = command.Version
            };

            var projectDefects = await defectRepository.GetDefectsDuplicatedViewModelByProjectAsync(new Guid(command.ProjectId), cancellationToken);

            var possibleDuplicates = Process.ExtractSorted(
                newDefect,
                projectDefects,
                d => $"{d.Summary} {d.Description} {d.Category} {d.Severity} {d.Environment} {d.Version}"
            );

            var duplicates = possibleDuplicates
                .Where(d => d.Score >= 60) 
                .Select(d => new DefectDuplicatesViewModel
                {
                    DefectId = d.Value.DefectId,
                    ProjectId = d.Value.ProjectId,
                    AssignedToUserId = d.Value.AssignedToUserId,
                    Summary = d.Value.Summary,
                    Description = d.Value.Description,
                    Category = d.Value.Category,
                    Severity = d.Value.Severity,
                    Environment = d.Value.Environment,
                    Version = d.Value.Version,
                    Score = d.Score
                })
                .ToList();

            return new DetectDefectDuplicatesResponse(duplicates.Count, duplicates);
        }
    }
}
