using Microsoft.EntityFrameworkCore.Migrations;

namespace LAHGO.Data.Migrations
{
    public partial class OrderItemUpdate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Orders");
        }
    }
}
