using Microsoft.EntityFrameworkCore.Migrations;

namespace LAHGO.Data.Migrations
{
    public partial class OrderItemUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Orders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
