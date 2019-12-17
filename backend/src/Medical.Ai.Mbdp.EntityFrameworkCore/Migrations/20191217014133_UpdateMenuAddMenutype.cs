using Microsoft.EntityFrameworkCore.Migrations;

namespace Medical.Ai.Mbdp.EntityFrameworkCore.Migrations
{
    public partial class UpdateMenuAddMenutype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MenuType",
                table: "MbpMenus",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "MbpUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "94c5fb886bd3cf5f821d239056181a5e");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MenuType",
                table: "MbpMenus");

            migrationBuilder.UpdateData(
                table: "MbpUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: null);
        }
    }
}
