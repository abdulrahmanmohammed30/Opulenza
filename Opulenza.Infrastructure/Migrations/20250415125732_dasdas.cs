using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Opulenza.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class dasdas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductMetadata_ProductId",
                table: "ProductMetadata");

            migrationBuilder.AddColumn<string>(
                name: "PaymentServiceId",
                table: "Products",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "ProductMetadata",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "ProductMetadata",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMetadata_ProductId",
                table: "ProductMetadata",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductMetadata_ProductId",
                table: "ProductMetadata");

            migrationBuilder.DropColumn(
                name: "PaymentServiceId",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "ProductMetadata",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "ProductMetadata",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60);

            migrationBuilder.CreateIndex(
                name: "IX_ProductMetadata_ProductId",
                table: "ProductMetadata",
                column: "ProductId",
                unique: true,
                filter: "[ProductId] IS NOT NULL");
        }
    }
}
