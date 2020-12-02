using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace J3space.Admin.Migrations
{
    public partial class RemovePermissionManagement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpPermissionGrants");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbpPermissionGrants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "varchar(128) CHARACTER SET utf8mb4", maxLength: 128,
                        nullable: false),
                    ProviderKey = table.Column<string>(type: "varchar(64) CHARACTER SET utf8mb4", maxLength: 64,
                        nullable: false),
                    ProviderName = table.Column<string>(type: "varchar(64) CHARACTER SET utf8mb4", maxLength: 64,
                        nullable: false),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_AbpPermissionGrants", x => x.Id); });

            migrationBuilder.CreateIndex(
                name: "IX_AbpPermissionGrants_Name_ProviderName_ProviderKey",
                table: "AbpPermissionGrants",
                columns: new[] {"Name", "ProviderName", "ProviderKey"});
        }
    }
}