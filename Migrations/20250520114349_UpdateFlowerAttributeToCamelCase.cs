using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCFlowerShop.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFlowerAttributeToCamelCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "flowerType",
                table: "FlowerTable",
                newName: "FlowerType");

            migrationBuilder.RenameColumn(
                name: "flowerProducedDate",
                table: "FlowerTable",
                newName: "FlowerProducedDate");

            migrationBuilder.RenameColumn(
                name: "flowerPrice",
                table: "FlowerTable",
                newName: "FlowerPrice");

            migrationBuilder.RenameColumn(
                name: "flowerName",
                table: "FlowerTable",
                newName: "FlowerName");

            migrationBuilder.RenameColumn(
                name: "flowerId",
                table: "FlowerTable",
                newName: "FlowerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FlowerType",
                table: "FlowerTable",
                newName: "flowerType");

            migrationBuilder.RenameColumn(
                name: "FlowerProducedDate",
                table: "FlowerTable",
                newName: "flowerProducedDate");

            migrationBuilder.RenameColumn(
                name: "FlowerPrice",
                table: "FlowerTable",
                newName: "flowerPrice");

            migrationBuilder.RenameColumn(
                name: "FlowerName",
                table: "FlowerTable",
                newName: "flowerName");

            migrationBuilder.RenameColumn(
                name: "FlowerId",
                table: "FlowerTable",
                newName: "flowerId");
        }
    }
}
