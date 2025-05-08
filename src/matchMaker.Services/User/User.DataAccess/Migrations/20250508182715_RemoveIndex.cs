using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserReports_ReporterUserId_ReportedUserId",
                table: "UserReports");

            migrationBuilder.CreateIndex(
                name: "IX_UserReports_ReporterUserId",
                table: "UserReports",
                column: "ReporterUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserReports_ReporterUserId",
                table: "UserReports");

            migrationBuilder.CreateIndex(
                name: "IX_UserReports_ReporterUserId_ReportedUserId",
                table: "UserReports",
                columns: new[] { "ReporterUserId", "ReportedUserId" },
                unique: true);
        }
    }
}
