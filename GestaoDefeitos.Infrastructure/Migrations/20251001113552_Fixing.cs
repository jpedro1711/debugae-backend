using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoDefeitos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fixing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Defects_AspNetUsers_AssignedToContributorId",
                table: "Defects");

            migrationBuilder.CreateTable(
                name: "DefectMailLetters",
                columns: table => new
                {
                    ContributorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DefectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefectMailLetters", x => new { x.DefectId, x.ContributorId });
                    table.ForeignKey(
                        name: "FK_DefectMailLetters_AspNetUsers_ContributorId",
                        column: x => x.ContributorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DefectMailLetters_Defects_DefectId",
                        column: x => x.DefectId,
                        principalTable: "Defects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DefectMailLetters_ContributorId",
                table: "DefectMailLetters",
                column: "ContributorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Defects_AspNetUsers_AssignedToContributorId",
                table: "Defects",
                column: "AssignedToContributorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Defects_AspNetUsers_AssignedToContributorId",
                table: "Defects");

            migrationBuilder.DropTable(
                name: "DefectMailLetters");

            migrationBuilder.AddForeignKey(
                name: "FK_Defects_AspNetUsers_AssignedToContributorId",
                table: "Defects",
                column: "AssignedToContributorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
