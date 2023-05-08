using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations.Ms
{
    /// <inheritdoc />
    public partial class _202304281407 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_GameId",
                table: "Campaigns",
                column: "GameId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_Games_GameId",
                table: "Campaigns",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_Games_GameId",
                table: "Campaigns");

            migrationBuilder.DropIndex(
                name: "IX_Campaigns_GameId",
                table: "Campaigns");
        }
    }
}
