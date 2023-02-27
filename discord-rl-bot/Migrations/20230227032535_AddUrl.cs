using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodyTedrick.Migrations
{
    /// <inheritdoc />
    public partial class AddUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountType",
                table: "UserInfo");

            migrationBuilder.DropColumn(
                name: "AccountTypeId",
                table: "UserInfo");

            migrationBuilder.RenameColumn(
                name: "GamerTag",
                table: "UserInfo",
                newName: "Url");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "UserInfo",
                newName: "GamerTag");

            migrationBuilder.AddColumn<int>(
                name: "AccountType",
                table: "UserInfo",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccountTypeId",
                table: "UserInfo",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
