using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCFlowerShop.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFlowerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlowerTable",
                columns: table => new
                {
                    flowerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    flowerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    flowerType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    flowerProducedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    flowerPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowerTable", x => x.flowerId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlowerTable");
        }
    }
}
