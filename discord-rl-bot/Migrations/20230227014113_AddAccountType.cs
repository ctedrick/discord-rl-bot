using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodyTedrick.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountType",
                table: "UserInfo");

            migrationBuilder.DropColumn(
                name: "AccountTypeId",
                table: "UserInfo");
        }
    }
}
