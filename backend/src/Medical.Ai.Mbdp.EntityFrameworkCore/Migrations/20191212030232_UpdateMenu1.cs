using Microsoft.EntityFrameworkCore.Migrations;

namespace Medical.Ai.Mbdp.EntityFrameworkCore.Migrations
{
    public partial class UpdateMenu1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasChildren",
                table: "MbpMenus",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "MbpMenus",
                keyColumn: "Id",
                keyValue: 1,
                column: "HasChildren",
                value: true);

            migrationBuilder.InsertData(
                table: "MbpMenus",
                columns: new[] { "Id", "Code", "CodePath", "ConcurrencyStamp", "HasChildren", "IsDeleted", "Level", "Name", "Order", "ParentId", "Path", "SystemCode" },
                values: new object[] { 3, "m20001", "root/m20001", null, true, false, 2, "大数据系统", 1, 1, "/", "mbdp" });

            migrationBuilder.InsertData(
                table: "MbpMenus",
                columns: new[] { "Id", "Code", "CodePath", "ConcurrencyStamp", "HasChildren", "IsDeleted", "Level", "Name", "Order", "ParentId", "Path", "SystemCode" },
                values: new object[] { 2, "m10001", "root/m10001", null, true, false, 2, "数据建模系统", 1, 1, "/", "mdp" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MbpMenus",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MbpMenus",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "HasChildren",
                table: "MbpMenus");
        }
    }
}
