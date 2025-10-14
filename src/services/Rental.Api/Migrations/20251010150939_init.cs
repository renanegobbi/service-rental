using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rental.Api.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "rental_service");

            migrationBuilder.CreateTable(
                name: "motorcycle",
                schema: "rental_service",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    year = table.Column<int>(type: "integer", nullable: false),
                    model = table.Column<string>(type: "varchar(50)", nullable: false),
                    plate = table.Column<string>(type: "varchar(10)", nullable: false),
                    created_at = table.Column<byte[]>(type: "timestamp", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_motorcycle", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "idx_motorcycle_plate",
                schema: "rental_service",
                table: "motorcycle",
                column: "plate",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "motorcycle",
                schema: "rental_service");
        }
    }
}
