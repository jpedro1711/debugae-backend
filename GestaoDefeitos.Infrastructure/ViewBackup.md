protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.Sql(@"
        IF OBJECT_ID('dbo.vw_DefectDetails', 'V') IS NOT NULL
            DROP VIEW dbo.vw_DefectDetails;
    ");

    migrationBuilder.Sql(@"
        CREATE VIEW vw_DefectDetails AS SELECT 
        d.Id as Id,
        d.Description,
        d.Summary,
        d.CreatedAt,
        CONCAT(c.Firstname, ' ', c.Lastname) AS CreatedBy,
        d.DefectSeverity,
        d.Status,
        d.ExpiresIn as ExpiresIn,
        d.DefectCategory,
        d.DefectEnvironment,
        d.AssignedToContributorId as AssignedTo,
        CONCAT(ac.Firstname, ' ', ac.Lastname) AS ContributorName,
        d.ExpectedBehaviour,
        d.ActualBehaviour,
        p.Name AS ProjectName,
        a.FileName AS AttachmentFileName,
        a.FileType AS AttachmentFileType,
        a.CreatedAt AS AttachmentCreatedAt,
        a.UploadByUsername,
        (
            SELECT 
                CONCAT(dc.Firstname, ' ', dc.Lastname) AS Author,
                com.Content,
                com.CreatedAt
            FROM DefectComments com
            INNER JOIN AspNetUsers dc ON dc.Id = com.ContributorId
            WHERE com.DefectId = d.Id
            FOR JSON PATH
        ) AS CommentsJson,
        (
            SELECT 
                rd.RelatedDefectId AS Id,
                r.Description,
                r.Summary,
                r.Status,
                r.DefectPriority,
                r.CreatedAt
            FROM DefectRelations rd
            INNER JOIN Defects r ON r.Id = rd.RelatedDefectId
            WHERE rd.DefectId = d.Id
            FOR JSON PATH
        ) AS RelatedDefectsJson,
        (
            SELECT 
                h.Action,
                h.CreatedAt,
                h.ContributorId,
                h.OldMetadataJson,
                h.NewMetadataJson
            FROM DefectHistory h
            INNER JOIN AspNetUsers c ON c.Id = h.ContributorId
            WHERE h.DefectId = d.Id
            ORDER BY h.CreatedAt
            FOR JSON PATH
        ) AS HistoryJson
        FROM Defects d
        RIGHT JOIN DefectHistory h ON h.DefectId = d.Id AND h.Action = 1
        RIGHT JOIN AspNetUsers c ON c.Id = h.ContributorId
        RIGHT JOIN AspNetUsers ac ON ac.Id = d.AssignedToContributorId
        RIGHT JOIN Projects p ON p.Id = d.ProjectId
        RIGHT JOIN DefectAttachments a ON a.DefectId = d.Id;
}

protected override void Down(MigrationBuilder migrationBuilder)
{
    migrationBuilder.Sql("DROP VIEW IF EXISTS dbo.vw_DefectDetails;");
}