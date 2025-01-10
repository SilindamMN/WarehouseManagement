using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProductQuantity = table.Column<int>(type: "int", nullable: false),
                    ProductDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WareHouses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WareHouseCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    WareHouseName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WareHouses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    SourceWareHouseId = table.Column<int>(type: "int", nullable: false),
                    DestinationWareHouseId = table.Column<int>(type: "int", nullable: false),
                    ProductQuantity = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_WareHouses_DestinationWareHouseId",
                        column: x => x.DestinationWareHouseId,
                        principalTable: "WareHouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_WareHouses_SourceWareHouseId",
                        column: x => x.SourceWareHouseId,
                        principalTable: "WareHouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductWareHouses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    WareHouseId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductWareHouses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductWareHouses_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductWareHouses_WareHouses_WareHouseId",
                        column: x => x.WareHouseId,
                        principalTable: "WareHouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "IsActive", "ProductCode", "ProductDescription", "ProductQuantity" },
                values: new object[,]
                {
                    { 1, true, "P001", "Product 1", 20 },
                    { 2, true, "P002", "Product 2", 20 },
                    { 3, true, "P003", "Product 3", 20 },
                    { 4, true, "P004", "Product 4", 20 },
                    { 5, true, "P005", "Product 5", 20 },
                    { 6, true, "P006", "Product 6", 20 },
                    { 7, true, "P007", "Product 7", 20 },
                    { 8, true, "P008", "Product 8", 20 },
                    { 9, true, "P009", "Product 9", 20 }
                });

            migrationBuilder.InsertData(
                table: "WareHouses",
                columns: new[] { "Id", "IsActive", "WareHouseCode", "WareHouseName" },
                values: new object[,]
                {
                    { 1, true, "WH001", "Warehouse 1" },
                    { 2, true, "WH002", "Warehouse 2" },
                    { 3, true, "WH003", "Warehouse 3" }
                });

            migrationBuilder.InsertData(
                table: "ProductWareHouses",
                columns: new[] { "Id", "IsActive", "ProductId", "Quantity", "WareHouseId" },
                values: new object[,]
                {
                    { 1, true, 1, 20, 1 },
                    { 2, true, 2, 20, 1 },
                    { 3, true, 3, 20, 2 },
                    { 4, true, 4, 20, 2 },
                    { 5, true, 5, 20, 3 },
                    { 6, true, 6, 20, 3 },
                    { 7, true, 7, 20, 1 },
                    { 8, true, 8, 20, 2 },
                    { 9, true, 9, 20, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DestinationWareHouseId",
                table: "Orders",
                column: "DestinationWareHouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ProductId",
                table: "Orders",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SourceWareHouseId",
                table: "Orders",
                column: "SourceWareHouseId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductWareHouses_ProductId",
                table: "ProductWareHouses",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductWareHouses_WareHouseId",
                table: "ProductWareHouses",
                column: "WareHouseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "ProductWareHouses");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "WareHouses");
        }
    }
}
