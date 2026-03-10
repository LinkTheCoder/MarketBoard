using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SubscribersWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subscribers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubscriptionNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DeliveryAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SubscriptionStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubscriptionEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubscriptionType = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscribers", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Subscribers",
                columns: new[] { "Id", "City", "CreatedAt", "DeliveryAddress", "Email", "FirstName", "IsActive", "LastName", "PhoneNumber", "PostalCode", "SubscriptionEndDate", "SubscriptionNumber", "SubscriptionStartDate", "SubscriptionType", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Stockholm", new DateTime(2025, 5, 17, 8, 41, 41, 700, DateTimeKind.Utc).AddTicks(8833), "Storgatan 10", "anna.andersson@email.se", "Anna", true, "Andersson", "070-123456", "12345", null, "SUB001", new DateTime(2025, 5, 17, 8, 41, 41, 700, DateTimeKind.Utc).AddTicks(8821), 4, new DateTime(2025, 5, 17, 8, 41, 41, 700, DateTimeKind.Utc).AddTicks(8834) },
                    { 2, "Göteborg", new DateTime(2025, 8, 17, 8, 41, 41, 700, DateTimeKind.Utc).AddTicks(8841), "Kungsgatan 25", "erik.eriksson@email.se", "Erik", true, "Eriksson", "070-789012", "41115", null, "SUB002", new DateTime(2025, 8, 17, 8, 41, 41, 700, DateTimeKind.Utc).AddTicks(8840), 1, new DateTime(2025, 8, 17, 8, 41, 41, 700, DateTimeKind.Utc).AddTicks(8842) },
                    { 3, "Malmö", new DateTime(2024, 11, 17, 8, 41, 41, 700, DateTimeKind.Utc).AddTicks(8853), "Drottninggatan 5", "maria.johansson@email.se", "Maria", true, "Johansson", "070-345678", "21121", null, "SUB003", new DateTime(2024, 11, 17, 8, 41, 41, 700, DateTimeKind.Utc).AddTicks(8847), 4, new DateTime(2024, 11, 17, 8, 41, 41, 700, DateTimeKind.Utc).AddTicks(8854) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscribers_SubscriptionNumber",
                table: "Subscribers",
                column: "SubscriptionNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subscribers");
        }
    }
}
