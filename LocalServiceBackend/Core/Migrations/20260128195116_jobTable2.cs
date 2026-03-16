using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trial.Migrations
{
    /// <inheritdoc />
    public partial class jobTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_ServiceCategories_ServiceCategoryCategoryId",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_ProviderServices_ServiceCategories_ServiceCategoryCategoryId",
                table: "ProviderServices");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_ServiceCategoryCategoryId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "ServiceCategoryCategoryId",
                table: "Jobs");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "ServiceCategories",
                newName: "ServiceCategoryId");

            migrationBuilder.RenameColumn(
                name: "ServiceCategoryCategoryId",
                table: "ProviderServices",
                newName: "ServiceCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ProviderServices_ServiceCategoryCategoryId",
                table: "ProviderServices",
                newName: "IX_ProviderServices_ServiceCategoryId");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Jobs",
                newName: "ServiceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_ServiceCategoryId",
                table: "Jobs",
                column: "ServiceCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_ServiceCategories_ServiceCategoryId",
                table: "Jobs",
                column: "ServiceCategoryId",
                principalTable: "ServiceCategories",
                principalColumn: "ServiceCategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProviderServices_ServiceCategories_ServiceCategoryId",
                table: "ProviderServices",
                column: "ServiceCategoryId",
                principalTable: "ServiceCategories",
                principalColumn: "ServiceCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_ServiceCategories_ServiceCategoryId",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_ProviderServices_ServiceCategories_ServiceCategoryId",
                table: "ProviderServices");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_ServiceCategoryId",
                table: "Jobs");

            migrationBuilder.RenameColumn(
                name: "ServiceCategoryId",
                table: "ServiceCategories",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "ServiceCategoryId",
                table: "ProviderServices",
                newName: "ServiceCategoryCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ProviderServices_ServiceCategoryId",
                table: "ProviderServices",
                newName: "IX_ProviderServices_ServiceCategoryCategoryId");

            migrationBuilder.RenameColumn(
                name: "ServiceCategoryId",
                table: "Jobs",
                newName: "CategoryId");

            migrationBuilder.AddColumn<int>(
                name: "ServiceCategoryCategoryId",
                table: "Jobs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_ServiceCategoryCategoryId",
                table: "Jobs",
                column: "ServiceCategoryCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_ServiceCategories_ServiceCategoryCategoryId",
                table: "Jobs",
                column: "ServiceCategoryCategoryId",
                principalTable: "ServiceCategories",
                principalColumn: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProviderServices_ServiceCategories_ServiceCategoryCategoryId",
                table: "ProviderServices",
                column: "ServiceCategoryCategoryId",
                principalTable: "ServiceCategories",
                principalColumn: "CategoryId");
        }
    }
}
