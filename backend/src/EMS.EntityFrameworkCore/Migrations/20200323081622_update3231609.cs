using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMS.EntityFrameworkCore.Migrations
{
    public partial class update3231609 : Migration
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
                name: "MbpCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ConcurrencyStamp = table.Column<DateTime>(rowVersion: true, nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    CategoryName = table.Column<string>(nullable: true),
                    CategoryCode = table.Column<string>(nullable: true),
                    ParentCategoryName = table.Column<string>(nullable: true),
                    ParentCategoryCode = table.Column<string>(nullable: true),
                    CategoryType = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    SystemCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MbpCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MbpDepts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ConcurrencyStamp = table.Column<DateTime>(rowVersion: true, nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    DeptName = table.Column<string>(maxLength: 256, nullable: true),
                    FullDeptName = table.Column<string>(maxLength: 1024, nullable: true),
                    DeptCode = table.Column<string>(maxLength: 256, nullable: true),
                    ParentDeptName = table.Column<string>(nullable: true),
                    ParentDeptCode = table.Column<string>(maxLength: 256, nullable: true),
                    ParentId = table.Column<int>(nullable: true),
                    DeptStatus = table.Column<int>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    SystemCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MbpDepts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MbpDepts_MbpDepts_ParentId",
                        column: x => x.ParentId,
                        principalTable: "MbpDepts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    MenuCompent = table.Column<string>(nullable: true),
                    MenuIcon = table.Column<string>(nullable: true),
                    IsEnabled = table.Column<bool>(nullable: false),
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
                name: "MbpPositions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ConcurrencyStamp = table.Column<DateTime>(rowVersion: true, nullable: true)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    PositionName = table.Column<string>(nullable: true),
                    PositionCode = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    SystemCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MbpPositions", x => x.Id);
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
                    DeptId = table.Column<int>(nullable: false),
                    DeptName = table.Column<string>(nullable: true),
                    UserSex = table.Column<int>(nullable: false),
                    Education = table.Column<int>(nullable: false),
                    Major = table.Column<string>(nullable: true),
                    UserType = table.Column<int>(nullable: false),
                    PositionType = table.Column<int>(nullable: false),
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
                name: "MbpUserDept",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    DeptId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MbpUserDept", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MbpUserDept_MbpDepts_DeptId",
                        column: x => x.DeptId,
                        principalTable: "MbpDepts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MbpUserDept_MbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "MbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MbpUserPositions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PositionId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MbpUserPositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MbpUserPositions_MbpPositions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "MbpPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MbpUserPositions_MbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "MbpUsers",
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
                table: "MbpCategories",
                columns: new[] { "Id", "CategoryCode", "CategoryName", "CategoryType", "IsDeleted", "ParentCategoryCode", "ParentCategoryName", "SystemCode" },
                values: new object[,]
                {
                    { 1, "f000001", "系统分类", 99, false, null, null, "Mbp" },
                    { 2, "f000002", "岗位分类", 1, false, "f000001", "系统分类", "Mbp" }
                });

            migrationBuilder.InsertData(
                table: "MbpDepts",
                columns: new[] { "Id", "DeptCode", "DeptName", "DeptStatus", "FullDeptName", "IsDeleted", "Level", "Order", "ParentDeptCode", "ParentDeptName", "ParentId", "SystemCode" },
                values: new object[] { 1, "d000001", "组织架构", 1, "组织架构", false, 0, 0, null, null, null, "Mbp" });

            migrationBuilder.InsertData(
                table: "MbpMenus",
                columns: new[] { "Id", "Code", "CodePath", "HasChildren", "IsDeleted", "IsEnabled", "Level", "MenuCompent", "MenuIcon", "MenuType", "Name", "Order", "ParentId", "Path", "SystemCode" },
                values: new object[] { 1, "root", "root", true, false, false, 1, null, null, 0, "Mbp平台", 1, 0, "/", "Mbp" });

            migrationBuilder.InsertData(
                table: "MbpUsers",
                columns: new[] { "Id", "Code", "DeptId", "DeptName", "Education", "Email", "IsAdmin", "IsDeleted", "LoginName", "Major", "Password", "PhoneNumber", "PositionType", "SystemCode", "UserName", "UserSex", "UserStatus", "UserType" },
                values: new object[] { 1, null, 0, null, 0, null, true, false, "admin", null, "94c5fb886bd3cf5f821d239056181a5e", null, 0, null, "admin", 0, 1, 0 });

            migrationBuilder.CreateIndex(
                name: "IX_MbpDepts_ParentId",
                table: "MbpDepts",
                column: "ParentId");

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
                name: "IX_MbpUserDept_DeptId",
                table: "MbpUserDept",
                column: "DeptId");

            migrationBuilder.CreateIndex(
                name: "IX_MbpUserDept_UserId",
                table: "MbpUserDept",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MbpUserPositions_PositionId",
                table: "MbpUserPositions",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_MbpUserPositions_UserId",
                table: "MbpUserPositions",
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
                name: "MbpCategories");

            migrationBuilder.DropTable(
                name: "MbpOperationLogs");

            migrationBuilder.DropTable(
                name: "MbpRoleMenus");

            migrationBuilder.DropTable(
                name: "MbpUserClaims");

            migrationBuilder.DropTable(
                name: "MbpUserDept");

            migrationBuilder.DropTable(
                name: "MbpUserPositions");

            migrationBuilder.DropTable(
                name: "MbpUserRoles");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "MbpMenuClaims");

            migrationBuilder.DropTable(
                name: "MbpDepts");

            migrationBuilder.DropTable(
                name: "MbpPositions");

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
