using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoDefeitos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRelationDefectHistory2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Defects_DefectAttachments_AttachmentId",
                table: "Defects");

            migrationBuilder.AlterColumn<Guid>(
                name: "AttachmentId",
                table: "Defects",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Defects_DefectAttachments_AttachmentId",
                table: "Defects",
                column: "AttachmentId",
                principalTable: "DefectAttachments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Defects_DefectAttachments_AttachmentId",
                table: "Defects");

            migrationBuilder.AlterColumn<Guid>(
                name: "AttachmentId",
                table: "Defects",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Defects_DefectAttachments_AttachmentId",
                table: "Defects",
                column: "AttachmentId",
                principalTable: "DefectAttachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
