using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Medical.Ai.Mbdp.EntityFrameworkCore.Migrations
{
    public partial class UpdateMenuAddMbpLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MbpOperationLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    RoleName = table.Column<string>(nullable: true),
                    ClientIP = table.Column<string>(nullable: true),
                    OpDateTime = table.Column<DateTime>(nullable: false),
                    AppName = table.Column<string>(nullable: true),
                    ModuleName = table.Column<string>(nullable: true),
                    OpName = table.Column<string>(nullable: true),
                    Desc = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MbpOperationLogs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MbpOperationLogs");
        }
    }
}
