using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Opulenza.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductDescriptionMaxCharacters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "varchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "varchar",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(max)");
        }
    }
}
