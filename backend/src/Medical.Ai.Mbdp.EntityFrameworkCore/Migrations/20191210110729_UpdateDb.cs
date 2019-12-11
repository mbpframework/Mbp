using Microsoft.EntityFrameworkCore.Migrations;

namespace Medical.Ai.Mbdp.EntityFrameworkCore.Migrations
{
    public partial class UpdateDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MbpMenus",
                columns: new[] { "Id", "Code", "ConcurrencyStamp", "IsDeleted", "Level", "Name", "Order", "ParentId", "Path", "SystemCode" },
                values: new object[] { 1, "root", null, false, 1, "医学大数据平台", 1, 0, "/", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MbpMenus",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
