#nullable disable

namespace Persistence.Migrations
{
  using Microsoft.EntityFrameworkCore.Migrations;
  using System;

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
                      onDelete: ReferentialAction.Restrict);
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
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_ProductWareHouses_WareHouses_WareHouseId",
                      column: x => x.WareHouseId,
                      principalTable: "WareHouses",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
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
          name: "IX_Products_ProductCode",
          table: "Products",
          column: "ProductCode",
          unique: true);

      migrationBuilder.CreateIndex(
          name: "IX_ProductWareHouses_ProductId",
          table: "ProductWareHouses",
          column: "ProductId");

      migrationBuilder.CreateIndex(
          name: "IX_ProductWareHouses_WareHouseId",
          table: "ProductWareHouses",
          column: "WareHouseId");

      migrationBuilder.CreateIndex(
          name: "IX_WareHouses_WareHouseCode",
          table: "WareHouses",
          column: "WareHouseCode",
          unique: true);
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