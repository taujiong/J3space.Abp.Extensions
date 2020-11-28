using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace J3space.Blogging.Migrations
{
    public partial class SimplifyTagEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "BlgTags");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "BlgTags");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "BlgTags");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "BlgTags");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "BlgTags");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "BlgTags");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "BlgTags");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "BlgTags");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "BlgTags");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "BlgTags",
                type: "varchar(40) CHARACTER SET utf8mb4",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "BlgTags",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "BlgTags",
                type: "char(36)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "BlgTags",
                type: "char(36)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "BlgTags",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "BlgTags",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "BlgTags",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "BlgTags",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "BlgTags",
                type: "char(36)",
                nullable: true);
        }
    }
}