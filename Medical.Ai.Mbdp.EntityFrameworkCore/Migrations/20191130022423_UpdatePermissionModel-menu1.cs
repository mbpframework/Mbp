using Microsoft.EntityFrameworkCore.Migrations;

namespace Medical.Ai.Mbdp.EntityFrameworkCore.Migrations
{
    public partial class UpdatePermissionModelmenu1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "MbpMenus",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "MbpMenus");
        }
    }
}
