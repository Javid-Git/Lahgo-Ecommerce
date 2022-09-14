using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LAHGO.Data.Migrations
{
    public partial class ProductTyping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductTypings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    ProductId = table.Column<int>(nullable: false),
                    TypingId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductTypings_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductTypings_Typings_TypingId",
                        column: x => x.TypingId,
                        principalTable: "Typings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypings_ProductId",
                table: "ProductTypings",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypings_TypingId",
                table: "ProductTypings",
                column: "TypingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductTypings");
        }
    }
}
