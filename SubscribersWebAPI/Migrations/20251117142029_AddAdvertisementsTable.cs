using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SubscribersWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddAdvertisementsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Advertisements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    ItemPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    AdvertisementPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AdvertiserType = table.Column<int>(type: "int", nullable: false),
                    SubscriptionNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OrganizationNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PublicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertisements", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Subscribers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "SubscriptionStartDate", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 17, 14, 20, 27, 973, DateTimeKind.Utc).AddTicks(600), new DateTime(2025, 5, 17, 14, 20, 27, 973, DateTimeKind.Utc).AddTicks(587), new DateTime(2025, 5, 17, 14, 20, 27, 973, DateTimeKind.Utc).AddTicks(603) });

            migrationBuilder.UpdateData(
                table: "Subscribers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "SubscriptionStartDate", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 17, 14, 20, 27, 973, DateTimeKind.Utc).AddTicks(617), new DateTime(2025, 8, 17, 14, 20, 27, 973, DateTimeKind.Utc).AddTicks(615), new DateTime(2025, 8, 17, 14, 20, 27, 973, DateTimeKind.Utc).AddTicks(620) });

            migrationBuilder.UpdateData(
                table: "Subscribers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "SubscriptionStartDate", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 11, 17, 14, 20, 27, 973, DateTimeKind.Utc).AddTicks(636), new DateTime(2024, 11, 17, 14, 20, 27, 973, DateTimeKind.Utc).AddTicks(629), new DateTime(2024, 11, 17, 14, 20, 27, 973, DateTimeKind.Utc).AddTicks(638) });

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_Category",
                table: "Advertisements",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_CreatedAt",
                table: "Advertisements",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_PublicationDate",
                table: "Advertisements",
                column: "PublicationDate");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_Status",
                table: "Advertisements",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Advertisements");

            migrationBuilder.UpdateData(
                table: "Subscribers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "SubscriptionStartDate", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 5, 17, 8, 41, 41, 700, DateTimeKind.Utc).AddTicks(8833), new DateTime(2025, 5, 17, 8, 41, 41, 700, DateTimeKind.Utc).AddTicks(8821), new DateTime(2025, 5, 17, 8, 41, 41, 700, DateTimeKind.Utc).AddTicks(8834) });

            migrationBuilder.UpdateData(
                table: "Subscribers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "SubscriptionStartDate", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 8, 17, 8, 41, 41, 700, DateTimeKind.Utc).AddTicks(8841), new DateTime(2025, 8, 17, 8, 41, 41, 700, DateTimeKind.Utc).AddTicks(8840), new DateTime(2025, 8, 17, 8, 41, 41, 700, DateTimeKind.Utc).AddTicks(8842) });

            migrationBuilder.UpdateData(
                table: "Subscribers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "SubscriptionStartDate", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 11, 17, 8, 41, 41, 700, DateTimeKind.Utc).AddTicks(8853), new DateTime(2024, 11, 17, 8, 41, 41, 700, DateTimeKind.Utc).AddTicks(8847), new DateTime(2024, 11, 17, 8, 41, 41, 700, DateTimeKind.Utc).AddTicks(8854) });
        }
    }
}
