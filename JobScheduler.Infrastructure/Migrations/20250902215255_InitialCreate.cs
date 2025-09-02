using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobScheduler.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobQueue",
                columns: table => new
                {
                    JobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobType = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Attempts = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    AvailableAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockedBy = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobQueue", x => x.JobId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobQueue_LockedAt",
                table: "JobQueue",
                column: "LockedAt");

            migrationBuilder.CreateIndex(
                name: "IX_JobQueue_Status_AvailableAt",
                table: "JobQueue",
                columns: new[] { "Status", "AvailableAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobQueue");
        }
    }
}
