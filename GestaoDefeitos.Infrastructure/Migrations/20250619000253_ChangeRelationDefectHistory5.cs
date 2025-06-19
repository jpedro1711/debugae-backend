using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoDefeitos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRelationDefectHistory5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DefectRelations",
                columns: table => new
                {
                    DefectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RelatedDefectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefectRelations", x => new { x.DefectId, x.RelatedDefectId });
                    table.ForeignKey(
                        name: "FK_DefectRelations_Defects_DefectId",
                        column: x => x.DefectId,
                        principalTable: "Defects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DefectRelations_Defects_RelatedDefectId",
                        column: x => x.RelatedDefectId,
                        principalTable: "Defects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DefectRelations_RelatedDefectId",
                table: "DefectRelations",
                column: "RelatedDefectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DefectRelations");
        }
    }
}
