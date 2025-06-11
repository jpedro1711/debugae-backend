namespace GestaoDefeitos.Domain.Entities
{
    public class DefectAttachment
    {
        public Guid Id { get; private set; }
        public string FileName { get; private set; }
        public string FileUri { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private DefectAttachment() { }

        public DefectAttachment(Guid id, string fileName, string fileUri)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentException("FileName is required");
            if (string.IsNullOrWhiteSpace(fileUri)) throw new ArgumentException("FileUri is required");

            Id = id;
            FileName = fileName;
            FileUri = fileUri;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentException("FileName is required");
            FileName = fileName;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateFileUri(string fileUri)
        {
            if (string.IsNullOrWhiteSpace(fileUri)) throw new ArgumentException("FileUri is required");
            FileUri = fileUri;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
