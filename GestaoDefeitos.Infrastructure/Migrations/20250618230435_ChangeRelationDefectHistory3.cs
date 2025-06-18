
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoDefeitos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRelationDefectHistory3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Defects_DefectId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_DefectId",
                table: "Tags");

            migrationBuilder.AlterColumn<Guid>(
                name: "DefectId",
                table: "Tags",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "DefectId",
                table: "Tags",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_DefectId",
                table: "Tags",
                column: "DefectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Defects_DefectId",
                table: "Tags",
                column: "DefectId",
                principalTable: "Defects",
                principalColumn: "Id");
        }
    }
}
