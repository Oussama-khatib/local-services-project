using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trial.Migrations
{
    /// <inheritdoc />
    public partial class ProviderServiceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProviderServices",
                columns: table => new
                {
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProviderId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    PriceMin = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PriceMax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceCategoryCategoryId = table.Column<int>(type: "int", nullable: true),
                    ServiceProviderProviderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderServices", x => x.ServiceId);
                    table.ForeignKey(
                        name: "FK_ProviderServices_ServiceCategories_ServiceCategoryCategoryId",
                        column: x => x.ServiceCategoryCategoryId,
                        principalTable: "ServiceCategories",
                        principalColumn: "CategoryId");
                    table.ForeignKey(
                        name: "FK_ProviderServices_ServiceProviders_ServiceProviderProviderId",
                        column: x => x.ServiceProviderProviderId,
                        principalTable: "ServiceProviders",
                        principalColumn: "ProviderId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProviderServices_ServiceCategoryCategoryId",
                table: "ProviderServices",
                column: "ServiceCategoryCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderServices_ServiceProviderProviderId",
                table: "ProviderServices",
                column: "ServiceProviderProviderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProviderServices");
        }
    }
}
