using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RL.Data.Migrations
{
    public partial class userPlanProcedureCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserPlanProcedure",
                columns: table => new
                {
                    UserPlanProcedureId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    PlanId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProcedureId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPlanProcedure", x => x.UserPlanProcedureId);
                    table.ForeignKey(
                        name: "FK_UserPlanProcedure_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPlanProcedure_Procedures_ProcedureId",
                        column: x => x.ProcedureId,
                        principalTable: "Procedures",
                        principalColumn: "ProcedureId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPlanProcedure_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPlanProcedure_PlanId",
                table: "UserPlanProcedure",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlanProcedure_ProcedureId",
                table: "UserPlanProcedure",
                column: "ProcedureId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlanProcedure_UserId_ProcedureId_PlanId_CreateDate_UpdateDate",
                table: "UserPlanProcedure",
                columns: new[] { "UserId", "ProcedureId", "PlanId", "CreateDate", "UpdateDate" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPlanProcedure");
        }
    }
}
