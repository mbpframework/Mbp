using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EMS.EntityFrameworkCore.Migrations
{
    public partial class update03240125 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ConcurrencyStamp",
                table: "MbpUsers",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ConcurrencyStamp",
                table: "MbpRoles",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<string>(
                name: "SystemCode",
                table: "MbpPositions",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PositionName",
                table: "MbpPositions",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PositionCode",
                table: "MbpPositions",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ConcurrencyStamp",
                table: "MbpPositions",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AddColumn<string>(
                name: "FullPositionName",
                table: "MbpPositions",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "MbpPositions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "MbpPositions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "MbpPositions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentPositionCode",
                table: "MbpPositions",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentPositionName",
                table: "MbpPositions",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PositionStatus",
                table: "MbpPositions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PositionType",
                table: "MbpPositions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ConcurrencyStamp",
                table: "MbpMenus",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ConcurrencyStamp",
                table: "MbpDepts",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ConcurrencyStamp",
                table: "MbpCategories",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ConcurrencyStamp",
                table: "Blogs",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.InsertData(
                table: "MbpPositions",
                columns: new[] { "Id", "FullPositionName", "IsDeleted", "Level", "Order", "ParentId", "ParentPositionCode", "ParentPositionName", "PositionCode", "PositionName", "PositionStatus", "PositionType", "SystemCode" },
                values: new object[] { 1, "岗位管理", false, 0, 0, null, null, null, "p000001", "岗位管理", 1, 0, "Mbp" });

            migrationBuilder.CreateIndex(
                name: "IX_MbpPositions_ParentId",
                table: "MbpPositions",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_MbpPositions_MbpPositions_ParentId",
                table: "MbpPositions",
                column: "ParentId",
                principalTable: "MbpPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MbpPositions_MbpPositions_ParentId",
                table: "MbpPositions");

            migrationBuilder.DropIndex(
                name: "IX_MbpPositions_ParentId",
                table: "MbpPositions");

            migrationBuilder.DeleteData(
                table: "MbpPositions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "FullPositionName",
                table: "MbpPositions");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "MbpPositions");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "MbpPositions");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "MbpPositions");

            migrationBuilder.DropColumn(
                name: "ParentPositionCode",
                table: "MbpPositions");

            migrationBuilder.DropColumn(
                name: "ParentPositionName",
                table: "MbpPositions");

            migrationBuilder.DropColumn(
                name: "PositionStatus",
                table: "MbpPositions");

            migrationBuilder.DropColumn(
                name: "PositionType",
                table: "MbpPositions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ConcurrencyStamp",
                table: "MbpUsers",
                type: "timestamp(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldRowVersion: true,
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ConcurrencyStamp",
                table: "MbpRoles",
                type: "timestamp(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldRowVersion: true,
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<string>(
                name: "SystemCode",
                table: "MbpPositions",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PositionName",
                table: "MbpPositions",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PositionCode",
                table: "MbpPositions",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ConcurrencyStamp",
                table: "MbpPositions",
                type: "timestamp(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldRowVersion: true,
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ConcurrencyStamp",
                table: "MbpMenus",
                type: "timestamp(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldRowVersion: true,
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ConcurrencyStamp",
                table: "MbpDepts",
                type: "timestamp(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldRowVersion: true,
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ConcurrencyStamp",
                table: "MbpCategories",
                type: "timestamp(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldRowVersion: true,
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ConcurrencyStamp",
                table: "Blogs",
                type: "timestamp(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldRowVersion: true,
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);
        }
    }
}
