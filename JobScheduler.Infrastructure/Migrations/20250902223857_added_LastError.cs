using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobScheduler.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class added_LastError : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastError",
                table: "JobQueue",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastError",
                table: "JobQueue");
        }
    }
}
