namespace GestaoDefeitos.Domain.Entities
{
    public class DefectMailLetter
    {
        public Guid ContributorId { get; set; }
        public Contributor Contributor { get; set; } = null!;

        public Guid DefectId { get; set; }
        public Defect Defect { get; set; } = null!;
    }
}
