using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMS.EntityFrameworkCore.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ConcurrencyStamp = table.Column<DateTime>(rowVersion: true, nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    Url = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MbpMenus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ConcurrencyStamp = table.Column<DateTime>(rowVersion: true, nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    CodePath = table.Column<string>(nullable: true),
                    Level = table.Column<int>(nullable: false),
                    Path = table.Column<string>(nullable: true),
                    ParentId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    SystemCode = table.Column<string>(nullable: true),
                    HasChildren = table.Column<bool>(nullable: false),
                    MenuType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MbpMenus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MbpOperationLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
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

            migrationBuilder.CreateTable(
                name: "MbpRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ConcurrencyStamp = table.Column<DateTime>(rowVersion: true, nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    Code = table.Column<string>(maxLength: 256, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    SystemCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MbpRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MbpUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ConcurrencyStamp = table.Column<DateTime>(rowVersion: true, nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    LoginName = table.Column<string>(maxLength: 256, nullable: true),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    Code = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    Password = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 256, nullable: true),
                    UserStatus = table.Column<int>(nullable: false),
                    IsAdmin = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    SystemCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MbpUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    BlogId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MbpMenuClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MenuId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MbpMenuClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MbpMenuClaims_MbpMenus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "MbpMenus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MbpRoleMenus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<int>(nullable: false),
                    MenuId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MbpRoleMenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MbpRoleMenus_MbpMenus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "MbpMenus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MbpRoleMenus_MbpRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "MbpRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MbpUserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MbpUserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MbpUserRoles_MbpRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "MbpRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MbpUserRoles_MbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "MbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MbpUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    MenuClaimId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MbpUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MbpUserClaims_MbpMenuClaims_MenuClaimId",
                        column: x => x.MenuClaimId,
                        principalTable: "MbpMenuClaims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MbpUserClaims_MbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "MbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MbpMenus",
                columns: new[] { "Id", "Code", "CodePath", "HasChildren", "IsDeleted", "Level", "MenuType", "Name", "Order", "ParentId", "Path", "SystemCode" },
                values: new object[,]
                {
                    { 1, "root", "root", true, false, 1, 0, "医学大数据平台", 1, 0, "/", null },
                    { 2, "m10001", "root/m10001", true, false, 2, 0, "数据建模系统", 1, 1, "/", "mdp" },
                    { 3, "m20001", "root/m20001", true, false, 2, 0, "大数据系统", 1, 1, "/", "mbdp" }
                });

            migrationBuilder.InsertData(
                table: "MbpUsers",
                columns: new[] { "Id", "Code", "Email", "IsAdmin", "IsDeleted", "LoginName", "Password", "PhoneNumber", "SystemCode", "UserName", "UserStatus" },
                values: new object[] { 1, null, null, true, false, "admin", "94c5fb886bd3cf5f821d239056181a5e", null, null, "admin", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_MbpMenuClaims_MenuId",
                table: "MbpMenuClaims",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_MbpRoleMenus_MenuId",
                table: "MbpRoleMenus",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_MbpRoleMenus_RoleId",
                table: "MbpRoleMenus",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_MbpUserClaims_MenuClaimId",
                table: "MbpUserClaims",
                column: "MenuClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_MbpUserClaims_UserId",
                table: "MbpUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MbpUserRoles_RoleId",
                table: "MbpUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_MbpUserRoles_UserId",
                table: "MbpUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_BlogId",
                table: "Posts",
                column: "BlogId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MbpOperationLogs");

            migrationBuilder.DropTable(
                name: "MbpRoleMenus");

            migrationBuilder.DropTable(
                name: "MbpUserClaims");

            migrationBuilder.DropTable(
                name: "MbpUserRoles");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "MbpMenuClaims");

            migrationBuilder.DropTable(
                name: "MbpRoles");

            migrationBuilder.DropTable(
                name: "MbpUsers");

            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "MbpMenus");
        }
    }
}
