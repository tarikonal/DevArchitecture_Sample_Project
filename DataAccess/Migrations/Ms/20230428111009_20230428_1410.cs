using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations.Ms
{
    /// <inheritdoc />
    public partial class _202304281410 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_Games_GameId",
                table: "Campaigns");

            migrationBuilder.DropIndex(
                name: "IX_Campaigns_GameId",
                table: "Campaigns");

            migrationBuilder.AlterColumn<int>(
                name: "GameId",
                table: "Campaigns",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_GameId",
                table: "Campaigns",
                column: "GameId",
                unique: true,
                filter: "[GameId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_Games_GameId",
                table: "Campaigns",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id");
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

            migrationBuilder.AlterColumn<int>(
                name: "GameId",
                table: "Campaigns",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
    }
}
