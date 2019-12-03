using Microsoft.EntityFrameworkCore.Migrations;

namespace Medical.Ai.Mbdp.EntityFrameworkCore.Migrations
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
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Level = table.Column<int>(nullable: false),
                    Path = table.Column<string>(nullable: true),
                    ParentId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MbpMenus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MbpRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    Code = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoginName = table.Column<string>(maxLength: 256, nullable: true),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    Code = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    Password = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(maxLength: 256, nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 256, nullable: true),
                    UserStatus = table.Column<int>(nullable: false),
                    IsAdmin = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
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
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
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

            migrationBuilder.InsertData(
                table: "MbpUsers",
                columns: new[] { "Id", "Code", "ConcurrencyStamp", "Email", "IsAdmin", "IsDeleted", "LoginName", "Password", "PhoneNumber", "UserName", "UserStatus" },
                values: new object[] { 1, null, null, null, true, false, "admin", null, null, "admin", 1 });

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
                name: "MbpMenuClaims");

            migrationBuilder.DropTable(
                name: "MbpRoleMenus");

            migrationBuilder.DropTable(
                name: "MbpUserRoles");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "MbpMenus");

            migrationBuilder.DropTable(
                name: "MbpRoles");

            migrationBuilder.DropTable(
                name: "MbpUsers");

            migrationBuilder.DropTable(
                name: "Blogs");
        }
    }
}
