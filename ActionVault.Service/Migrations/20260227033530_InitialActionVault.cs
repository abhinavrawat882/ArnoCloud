using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ActionVault.Service.Migrations
{
    /// <inheritdoc />
    public partial class InitialActionVault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "actionvault");

            migrationBuilder.CreateTable(
                name: "VaultTasks",
                schema: "actionvault",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkspaceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    IsRecurring = table.Column<bool>(type: "boolean", nullable: false),
                    RecurrenceType = table.Column<int>(type: "integer", nullable: false),
                    RecurrenceIntervalMinutes = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaultTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DailyQueues",
                schema: "actionvault",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VaultTaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    SelectedDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyQueues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyQueues_VaultTasks_VaultTaskId",
                        column: x => x.VaultTaskId,
                        principalSchema: "actionvault",
                        principalTable: "VaultTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyQueues_VaultTaskId_SelectedDate",
                schema: "actionvault",
                table: "DailyQueues",
                columns: new[] { "VaultTaskId", "SelectedDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VaultTasks_WorkspaceId",
                schema: "actionvault",
                table: "VaultTasks",
                column: "WorkspaceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyQueues",
                schema: "actionvault");

            migrationBuilder.DropTable(
                name: "VaultTasks",
                schema: "actionvault");
        }
    }
}
