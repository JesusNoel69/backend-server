using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEnd_Server.Migrations
{
    /// <inheritdoc />
    public partial class DeveloperRelatedTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeveloperId",
                table: "TaskEntity",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskEntity_DeveloperId",
                table: "TaskEntity",
                column: "DeveloperId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskEntity_Developer_DeveloperId",
                table: "TaskEntity",
                column: "DeveloperId",
                principalTable: "Developer",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskEntity_Developer_DeveloperId",
                table: "TaskEntity");

            migrationBuilder.DropIndex(
                name: "IX_TaskEntity_DeveloperId",
                table: "TaskEntity");

            migrationBuilder.DropColumn(
                name: "DeveloperId",
                table: "TaskEntity");
        }
    }
}
