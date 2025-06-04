using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCFlowerShop.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFlowerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlowerTable");

            migrationBuilder.CreateTable(
                name: "FlowerList",
                columns: table => new
                {
                    FlowerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlowerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlowerType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlowerProducedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FlowerPrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowerList", x => x.FlowerId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlowerList");

            migrationBuilder.CreateTable(
                name: "FlowerTable",
                columns: table => new
                {
                    FlowerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlowerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlowerPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FlowerProducedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FlowerType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowerTable", x => x.FlowerId);
                });
        }
    }
}
