using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEnd_Server.Migrations
{
    /// <inheritdoc />
    public partial class ChangingModelChangeDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ChangeDetails",
                newName: "TaskId1");

            migrationBuilder.RenameIndex(
                name: "IX_ChangeDetails_UserId",
                table: "ChangeDetails",
                newName: "IX_ChangeDetails_TaskId1");

            migrationBuilder.AddColumn<int>(
                name: "ChangeDetailsId",
                table: "ProductOwner",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TaskId",
                table: "ChangeDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeveloperId",
                table: "ChangeDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DeveloperId1",
                table: "ChangeDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SprintId1",
                table: "ChangeDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductOwner_ChangeDetailsId",
                table: "ProductOwner",
                column: "ChangeDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeDetails_DeveloperId",
                table: "ChangeDetails",
                column: "DeveloperId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeDetails_DeveloperId1",
                table: "ChangeDetails",
                column: "DeveloperId1",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChangeDetails_SprintId",
                table: "ChangeDetails",
                column: "SprintId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeDetails_SprintId1",
                table: "ChangeDetails",
                column: "SprintId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ChangeDetails_Developer_DeveloperId",
                table: "ChangeDetails",
                column: "DeveloperId",
                principalTable: "Developer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChangeDetails_Developer_DeveloperId1",
                table: "ChangeDetails",
                column: "DeveloperId1",
                principalTable: "Developer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChangeDetails_Sprint_SprintId",
                table: "ChangeDetails",
                column: "SprintId",
                principalTable: "Sprint",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChangeDetails_Sprint_SprintId1",
                table: "ChangeDetails",
                column: "SprintId1",
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
                name: "FK_ChangeDetails_TaskEntity_TaskId1",
                table: "ChangeDetails",
                column: "TaskId1",
                principalTable: "TaskEntity",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOwner_ChangeDetails_ChangeDetailsId",
                table: "ProductOwner",
                column: "ChangeDetailsId",
                principalTable: "ChangeDetails",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChangeDetails_Developer_DeveloperId",
                table: "ChangeDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ChangeDetails_Developer_DeveloperId1",
                table: "ChangeDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ChangeDetails_Sprint_SprintId",
                table: "ChangeDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ChangeDetails_Sprint_SprintId1",
                table: "ChangeDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ChangeDetails_TaskEntity_TaskId",
                table: "ChangeDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ChangeDetails_TaskEntity_TaskId1",
                table: "ChangeDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOwner_ChangeDetails_ChangeDetailsId",
                table: "ProductOwner");

            migrationBuilder.DropIndex(
                name: "IX_ProductOwner_ChangeDetailsId",
                table: "ProductOwner");

            migrationBuilder.DropIndex(
                name: "IX_ChangeDetails_DeveloperId",
                table: "ChangeDetails");

            migrationBuilder.DropIndex(
                name: "IX_ChangeDetails_DeveloperId1",
                table: "ChangeDetails");

            migrationBuilder.DropIndex(
                name: "IX_ChangeDetails_SprintId",
                table: "ChangeDetails");

            migrationBuilder.DropIndex(
                name: "IX_ChangeDetails_SprintId1",
                table: "ChangeDetails");

            migrationBuilder.DropColumn(
                name: "ChangeDetailsId",
                table: "ProductOwner");

            migrationBuilder.DropColumn(
                name: "DeveloperId",
                table: "ChangeDetails");

            migrationBuilder.DropColumn(
                name: "DeveloperId1",
                table: "ChangeDetails");

            migrationBuilder.DropColumn(
                name: "SprintId1",
                table: "ChangeDetails");

            migrationBuilder.RenameColumn(
                name: "TaskId1",
                table: "ChangeDetails",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ChangeDetails_TaskId1",
                table: "ChangeDetails",
                newName: "IX_ChangeDetails_UserId");

            migrationBuilder.AddColumn<int>(
                name: "ChangeDetailsId",
                table: "User",
                type: "int",
                nullable: true);

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
    }
}
