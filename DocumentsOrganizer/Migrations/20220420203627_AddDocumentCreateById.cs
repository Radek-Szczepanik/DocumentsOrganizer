using Microsoft.EntityFrameworkCore.Migrations;

namespace DocumentsOrganizer.Migrations
{
    public partial class AddDocumentCreateById : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Documents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_CreatedById",
                table: "Documents",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Users_CreatedById",
                table: "Documents",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Users_CreatedById",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_CreatedById",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Documents");
        }
    }
}
