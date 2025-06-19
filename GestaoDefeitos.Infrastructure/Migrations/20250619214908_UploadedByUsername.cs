using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoDefeitos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UploadedByUsername : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UploadByUsername",
                table: "DefectAttachments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UploadByUsername",
                table: "DefectAttachments");
        }
    }
}
