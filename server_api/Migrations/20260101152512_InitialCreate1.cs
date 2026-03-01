using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server_api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDrawn",
                table: "Gifts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "WinnerUserId",
                table: "Gifts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gifts_WinnerUserId",
                table: "Gifts",
                column: "WinnerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gifts_Users_WinnerUserId",
                table: "Gifts",
                column: "WinnerUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gifts_Users_WinnerUserId",
                table: "Gifts");

            migrationBuilder.DropIndex(
                name: "IX_Gifts_WinnerUserId",
                table: "Gifts");

            migrationBuilder.DropColumn(
                name: "IsDrawn",
                table: "Gifts");

            migrationBuilder.DropColumn(
                name: "WinnerUserId",
                table: "Gifts");
        }
    }
}
