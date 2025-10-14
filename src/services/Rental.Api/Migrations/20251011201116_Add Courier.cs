using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rental.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddCourier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "courier",
                schema: "rental_service",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    full_name = table.Column<string>(type: "varchar(100)", nullable: false),
                    cnpj = table.Column<string>(type: "varchar(18)", nullable: false),
                    birth_date = table.Column<DateTime>(type: "date", nullable: false),
                    driver_license_number = table.Column<string>(type: "varchar(20)", nullable: false),
                    driver_license_type = table.Column<string>(type: "varchar(5)", nullable: false),
                    driver_license_image_url = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<byte[]>(type: "timestamp", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courier", x => x.Id);
                    table.CheckConstraint("chk_courier_license_type", "driver_license_type IN ('A', 'B', 'AB')");
                });

            migrationBuilder.CreateIndex(
                name: "idx_courier_cnpj",
                schema: "rental_service",
                table: "courier",
                column: "cnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_courier_license_number",
                schema: "rental_service",
                table: "courier",
                column: "driver_license_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_courier_name",
                schema: "rental_service",
                table: "courier",
                column: "full_name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "courier",
                schema: "rental_service");
        }
    }
}
