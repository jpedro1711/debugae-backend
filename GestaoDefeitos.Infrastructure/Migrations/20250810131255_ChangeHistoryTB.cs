using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoDefeitos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeHistoryTB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewMetadataJson",
                table: "DefectHistory");

            migrationBuilder.DropColumn(
                name: "OldMetadataJson",
                table: "DefectHistory");

            migrationBuilder.AddColumn<string>(
                name: "Field",
                table: "DefectHistory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewValue",
                table: "DefectHistory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OldValue",
                table: "DefectHistory",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Field",
                table: "DefectHistory");

            migrationBuilder.DropColumn(
                name: "NewValue",
                table: "DefectHistory");

            migrationBuilder.DropColumn(
                name: "OldValue",
                table: "DefectHistory");

            migrationBuilder.AddColumn<string>(
                name: "NewMetadataJson",
                table: "DefectHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OldMetadataJson",
                table: "DefectHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
