using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Opulenza.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class usp_GetCategoryRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.Sql(@"
                                CREATE PROC dbo.usp_HasCircularDependency
                                    @parentCategoryId INT,
                                    @categoryId INT
                                AS
                                BEGIN
                                    SET NOCOUNT ON;
                                
                                    WITH RecursiveCTE AS (
                                         SELECT Id, ParentId
                                         FROM Categories
                                         WHERE Id = @parentCategoryId
                                         UNION ALL
                                         SELECT c.Id, c.ParentId
                                         FROM Categories c
                                         INNER JOIN RecursiveCTE r ON c.Id = r.ParentId
                                    )
                                    SELECT Id, ParentId
                                    FROM RecursiveCTE
                                    WHERE Id = @categoryId;
                                END
                              ");
        }
        
        

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_Name",
                table: "Categories");
            
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.usp_HasCircularDependency");
        }
    }
}
