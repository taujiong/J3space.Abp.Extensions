using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace J3space.Auth.Migrations
{
    public partial class AddPermissionManagement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbpPermissionGrants",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderName = table.Column<string>(maxLength: 64, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_AbpPermissionGrants", x => x.Id); });

            migrationBuilder.CreateIndex(
                name: "IX_AbpPermissionGrants_Name_ProviderName_ProviderKey",
                table: "AbpPermissionGrants",
                columns: new[] {"Name", "ProviderName", "ProviderKey"});
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpPermissionGrants");
        }
    }
}