using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEnd_Server.Migrations
{
    /// <inheritdoc />
    public partial class ConvertToListsProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChangeDetails_Sprint_SprintId",
                table: "ChangeDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ChangeDetails_TaskEntity_TaskId",
                table: "ChangeDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ChangeDetails_User_UserId",
                table: "ChangeDetails");

            migrationBuilder.DropIndex(
                name: "IX_ChangeDetails_SprintId",
                table: "ChangeDetails");

            migrationBuilder.AddColumn<int>(
                name: "ChangeDetailsId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "ChangeDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TaskId",
                table: "ChangeDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "ChangeDetailsSprint",
                columns: table => new
                {
                    ChangeDetailsId = table.Column<int>(type: "int", nullable: false),
                    SprintsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeDetailsSprint", x => new { x.ChangeDetailsId, x.SprintsId });
                    table.ForeignKey(
                        name: "FK_ChangeDetailsSprint_ChangeDetails_ChangeDetailsId",
                        column: x => x.ChangeDetailsId,
                        principalTable: "ChangeDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChangeDetailsSprint_Sprint_SprintsId",
                        column: x => x.SprintsId,
                        principalTable: "Sprint",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ChangeDetailsTask",
                columns: table => new
                {
                    ChangeDetailsId = table.Column<int>(type: "int", nullable: false),
                    TaskEntitiesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeDetailsTask", x => new { x.ChangeDetailsId, x.TaskEntitiesId });
                    table.ForeignKey(
                        name: "FK_ChangeDetailsTask_ChangeDetails_ChangeDetailsId",
                        column: x => x.ChangeDetailsId,
                        principalTable: "ChangeDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChangeDetailsTask_TaskEntity_TaskEntitiesId",
                        column: x => x.TaskEntitiesId,
                        principalTable: "TaskEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_User_ChangeDetailsId",
                table: "User",
                column: "ChangeDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeDetailsSprint_SprintsId",
                table: "ChangeDetailsSprint",
                column: "SprintsId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeDetailsTask_TaskEntitiesId",
                table: "ChangeDetailsTask",
                column: "TaskEntitiesId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_ChangeDetails_ChangeDetailsId",
                table: "User",
                column: "ChangeDetailsId",
                principalTable: "ChangeDetails",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_ChangeDetails_ChangeDetailsId",
                table: "User");

            migrationBuilder.DropTable(
                name: "ChangeDetailsSprint");

            migrationBuilder.DropTable(
                name: "ChangeDetailsTask");

            migrationBuilder.DropIndex(
                name: "IX_User_ChangeDetailsId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ChangeDetailsId",
                table: "User");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "ChangeDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TaskId",
                table: "ChangeDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChangeDetails_SprintId",
                table: "ChangeDetails",
                column: "SprintId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChangeDetails_Sprint_SprintId",
                table: "ChangeDetails",
                column: "SprintId",
                principalTable: "Sprint",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChangeDetails_TaskEntity_TaskId",
                table: "ChangeDetails",
                column: "TaskId",
                principalTable: "TaskEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChangeDetails_User_UserId",
                table: "ChangeDetails",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
