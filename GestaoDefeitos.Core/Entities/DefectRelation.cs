namespace GestaoDefeitos.Domain.Entities
{
    public class DefectRelation
    {
        public Guid DefectId { get; set; }
        public Defect Defect { get; set; } = null!;

        public Guid RelatedDefectId { get; set; }
        public Defect RelatedDefect { get; set; } = null!;
    }
}
