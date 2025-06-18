using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profile.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileInterest_Interests_InterestsId",
                table: "ProfileInterest");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileInterest_Profiles_ProfilesId",
                table: "ProfileInterest");

            migrationBuilder.DropTable(
                name: "ProfileLanguage");

            migrationBuilder.RenameColumn(
                name: "ProfilesId",
                table: "ProfileInterest",
                newName: "ProfileId");

            migrationBuilder.RenameColumn(
                name: "InterestsId",
                table: "ProfileInterest",
                newName: "InterestId");

            migrationBuilder.RenameIndex(
                name: "IX_ProfileInterest_ProfilesId",
                table: "ProfileInterest",
                newName: "IX_ProfileInterest_ProfileId");

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
                name: "FK_ProfileInterest_Interest_InterestId",
                table: "ProfileInterest",
                column: "InterestId",
                principalTable: "Interests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileInterest_UserProfile_ProfileId",
                table: "ProfileInterest",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileLanguage_Language_InterestId",
                table: "ProfileInterest",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileInterest_Interest_InterestId",
                table: "ProfileInterest");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileInterest_UserProfile_ProfileId",
                table: "ProfileInterest");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileLanguage_Language_InterestId",
                table: "ProfileInterest");

            migrationBuilder.DropIndex(
                name: "IX_ProfileInterest_LanguageId",
                table: "ProfileInterest");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "ProfileInterest");

            migrationBuilder.RenameColumn(
                name: "ProfileId",
                table: "ProfileInterest",
                newName: "ProfilesId");

            migrationBuilder.RenameColumn(
                name: "InterestId",
                table: "ProfileInterest",
                newName: "InterestsId");

            migrationBuilder.RenameIndex(
                name: "IX_ProfileInterest_ProfileId",
                table: "ProfileInterest",
                newName: "IX_ProfileInterest_ProfilesId");

            migrationBuilder.CreateTable(
                name: "ProfileLanguage",
                columns: table => new
                {
                    LanguagesId = table.Column<long>(type: "bigint", nullable: false),
                    ProfilesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileLanguage", x => new { x.LanguagesId, x.ProfilesId });
                    table.ForeignKey(
                        name: "FK_ProfileLanguage_Languages_LanguagesId",
                        column: x => x.LanguagesId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfileLanguage_Profiles_ProfilesId",
                        column: x => x.ProfilesId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileLanguage_ProfilesId",
                table: "ProfileLanguage",
                column: "ProfilesId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileInterest_Interests_InterestsId",
                table: "ProfileInterest",
                column: "InterestsId",
                principalTable: "Interests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileInterest_Profiles_ProfilesId",
                table: "ProfileInterest",
                column: "ProfilesId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
