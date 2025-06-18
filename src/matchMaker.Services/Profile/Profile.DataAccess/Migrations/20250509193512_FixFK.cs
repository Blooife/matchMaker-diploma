using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profile.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileInterest_UserProfile_ProfileId",
                table: "ProfileInterest");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileLanguage_UserProfile_ProfileId",
                table: "ProfileInterest",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileLanguage_UserProfile_ProfileId",
                table: "ProfileInterest");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileInterest_UserProfile_ProfileId",
                table: "ProfileInterest",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
