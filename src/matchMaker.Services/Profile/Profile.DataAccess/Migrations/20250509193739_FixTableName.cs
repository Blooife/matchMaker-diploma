using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profile.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileLanguage_Language_InterestId",
                table: "ProfileInterest");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileLanguage_UserProfile_ProfileId",
                table: "ProfileInterest");

            migrationBuilder.DropIndex(
                name: "IX_ProfileInterest_LanguageId",
                table: "ProfileInterest");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "ProfileInterest");

            migrationBuilder.CreateTable(
                name: "ProfileLanguage",
                columns: table => new
                {
                    LanguageId = table.Column<long>(type: "bigint", nullable: false),
                    ProfileId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileLanguage", x => new { x.LanguageId, x.ProfileId });
                    table.ForeignKey(
                        name: "FK_ProfileLanguage_Language_InterestId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfileLanguage_UserProfile_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileLanguage_ProfileId",
                table: "ProfileLanguage",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileInterest_UserProfile_ProfileId",
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
                name: "FK_ProfileInterest_UserProfile_ProfileId",
                table: "ProfileInterest");

            migrationBuilder.DropTable(
                name: "ProfileLanguage");

            migrationBuilder.AddColumn<long>(
                name: "LanguageId",
                table: "ProfileInterest",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfileInterest_LanguageId",
                table: "ProfileInterest",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileLanguage_Language_InterestId",
                table: "ProfileInterest",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id");
        }
    }
}
