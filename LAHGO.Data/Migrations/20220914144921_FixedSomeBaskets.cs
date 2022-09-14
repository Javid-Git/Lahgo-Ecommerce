using Microsoft.EntityFrameworkCore.Migrations;

namespace LAHGO.Data.Migrations
{
    public partial class FixedSomeBaskets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BasketId",
                table: "Sizes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BasketId",
                table: "Colors",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sizes_BasketId",
                table: "Sizes",
                column: "BasketId");

            migrationBuilder.CreateIndex(
                name: "IX_Colors_BasketId",
                table: "Colors",
                column: "BasketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Colors_Baskets_BasketId",
                table: "Colors",
                column: "BasketId",
                principalTable: "Baskets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sizes_Baskets_BasketId",
                table: "Sizes",
                column: "BasketId",
                principalTable: "Baskets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Colors_Baskets_BasketId",
                table: "Colors");

            migrationBuilder.DropForeignKey(
                name: "FK_Sizes_Baskets_BasketId",
                table: "Sizes");

            migrationBuilder.DropIndex(
                name: "IX_Sizes_BasketId",
                table: "Sizes");

            migrationBuilder.DropIndex(
                name: "IX_Colors_BasketId",
                table: "Colors");

            migrationBuilder.DropColumn(
                name: "BasketId",
                table: "Sizes");

            migrationBuilder.DropColumn(
                name: "BasketId",
                table: "Colors");
        }
    }
}
