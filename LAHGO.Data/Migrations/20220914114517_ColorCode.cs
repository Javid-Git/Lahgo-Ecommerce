using Microsoft.EntityFrameworkCore.Migrations;

namespace LAHGO.Data.Migrations
{
    public partial class ColorCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Colors",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Colors");
        }
    }
}
