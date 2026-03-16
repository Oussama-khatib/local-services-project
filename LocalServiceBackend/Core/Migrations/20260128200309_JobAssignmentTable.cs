using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trial.Migrations
{
    /// <inheritdoc />
    public partial class JobAssignmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProviderServices_ServiceCategories_ServiceCategoryId",
                table: "ProviderServices");

            migrationBuilder.DropForeignKey(
                name: "FK_ProviderServices_ServiceProviders_ServiceProviderProviderId",
                table: "ProviderServices");

            migrationBuilder.DropIndex(
                name: "IX_ProviderServices_ServiceProviderProviderId",
                table: "ProviderServices");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "ProviderServices");

            migrationBuilder.DropColumn(
                name: "ServiceProviderProviderId",
                table: "ProviderServices");

            migrationBuilder.RenameColumn(
                name: "ProviderId",
                table: "ServiceProviders",
                newName: "ServiceProviderId");

            migrationBuilder.RenameColumn(
                name: "ProviderId",
                table: "ProviderServices",
                newName: "ServiceProviderId");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceCategoryId",
                table: "ProviderServices",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "JobAssignments",
                columns: table => new
                {
                    JobAssignmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceProviderId = table.Column<int>(type: "int", nullable: false),
                    JobId = table.Column<int>(type: "int", nullable: false),
                    AcceptedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobAssignments", x => x.JobAssignmentId);
                    table.ForeignKey(
                        name: "FK_JobAssignments_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "JobId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobAssignments_ServiceProviders_ServiceProviderId",
                        column: x => x.ServiceProviderId,
                        principalTable: "ServiceProviders",
                        principalColumn: "ServiceProviderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProviderServices_ServiceProviderId",
                table: "ProviderServices",
                column: "ServiceProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_JobAssignments_JobId",
                table: "JobAssignments",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_JobAssignments_ServiceProviderId",
                table: "JobAssignments",
                column: "ServiceProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProviderServices_ServiceCategories_ServiceCategoryId",
                table: "ProviderServices",
                column: "ServiceCategoryId",
                principalTable: "ServiceCategories",
                principalColumn: "ServiceCategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProviderServices_ServiceProviders_ServiceProviderId",
                table: "ProviderServices",
                column: "ServiceProviderId",
                principalTable: "ServiceProviders",
                principalColumn: "ServiceProviderId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProviderServices_ServiceCategories_ServiceCategoryId",
                table: "ProviderServices");

            migrationBuilder.DropForeignKey(
                name: "FK_ProviderServices_ServiceProviders_ServiceProviderId",
                table: "ProviderServices");

            migrationBuilder.DropTable(
                name: "JobAssignments");

            migrationBuilder.DropIndex(
                name: "IX_ProviderServices_ServiceProviderId",
                table: "ProviderServices");

            migrationBuilder.RenameColumn(
                name: "ServiceProviderId",
                table: "ServiceProviders",
                newName: "ProviderId");

            migrationBuilder.RenameColumn(
                name: "ServiceProviderId",
                table: "ProviderServices",
                newName: "ProviderId");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceCategoryId",
                table: "ProviderServices",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "ProviderServices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ServiceProviderProviderId",
                table: "ProviderServices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProviderServices_ServiceProviderProviderId",
                table: "ProviderServices",
                column: "ServiceProviderProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProviderServices_ServiceCategories_ServiceCategoryId",
                table: "ProviderServices",
                column: "ServiceCategoryId",
                principalTable: "ServiceCategories",
                principalColumn: "ServiceCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProviderServices_ServiceProviders_ServiceProviderProviderId",
                table: "ProviderServices",
                column: "ServiceProviderProviderId",
                principalTable: "ServiceProviders",
                principalColumn: "ProviderId");
        }
    }
}
