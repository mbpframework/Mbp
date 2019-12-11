using Microsoft.EntityFrameworkCore.Migrations;

namespace Medical.Ai.Mbdp.EntityFrameworkCore.Migrations
{
    public partial class UpdateMenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodePath",
                table: "MbpMenus",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "MbpMenus",
                keyColumn: "Id",
                keyValue: 1,
                column: "CodePath",
                value: "root");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodePath",
                table: "MbpMenus");
        }
    }
}
