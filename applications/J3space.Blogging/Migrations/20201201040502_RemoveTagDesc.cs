using Microsoft.EntityFrameworkCore.Migrations;

namespace J3space.Blogging.Migrations
{
    public partial class RemoveTagDesc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "BlgTags");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "BlgTags",
                type: "varchar(512) CHARACTER SET utf8mb4",
                maxLength: 512,
                nullable: true);
        }
    }
}