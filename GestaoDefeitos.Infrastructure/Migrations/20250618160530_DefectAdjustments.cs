using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoDefeitos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DefectAdjustments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DefectAttachments_Defects_DefectId",
                table: "DefectAttachments");

            migrationBuilder.DropIndex(
                name: "IX_DefectAttachments_DefectId",
                table: "DefectAttachments");

            migrationBuilder.DropColumn(
                name: "DefectId",
                table: "DefectAttachments");

            migrationBuilder.RenameColumn(
                name: "FileUri",
                table: "DefectAttachments",
                newName: "FileType");

            migrationBuilder.AddColumn<Guid>(
                name: "AttachmentId",
                table: "Defects",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "DefectPriority",
                table: "Defects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte[]>(
                name: "FileContent",
                table: "DefectAttachments",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Defects_AttachmentId",
                table: "Defects",
                column: "AttachmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Defects_DefectAttachments_AttachmentId",
                table: "Defects",
                column: "AttachmentId",
                principalTable: "DefectAttachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Defects_DefectAttachments_AttachmentId",
                table: "Defects");

            migrationBuilder.DropIndex(
                name: "IX_Defects_AttachmentId",
                table: "Defects");

            migrationBuilder.DropColumn(
                name: "AttachmentId",
                table: "Defects");

            migrationBuilder.DropColumn(
                name: "DefectPriority",
                table: "Defects");

            migrationBuilder.DropColumn(
                name: "FileContent",
                table: "DefectAttachments");

            migrationBuilder.RenameColumn(
                name: "FileType",
                table: "DefectAttachments",
                newName: "FileUri");

            migrationBuilder.AddColumn<Guid>(
                name: "DefectId",
                table: "DefectAttachments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DefectAttachments_DefectId",
                table: "DefectAttachments",
                column: "DefectId");

            migrationBuilder.AddForeignKey(
                name: "FK_DefectAttachments_Defects_DefectId",
                table: "DefectAttachments",
                column: "DefectId",
                principalTable: "Defects",
                principalColumn: "Id");
        }
    }
}
