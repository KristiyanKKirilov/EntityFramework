using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intro.Migrations
{
    /// <inheritdoc />
    public partial class AppartmentNumberAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppartmentNumber",
                table: "Addresses",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppartmentNumber",
                table: "Addresses");
        }
    }
}
