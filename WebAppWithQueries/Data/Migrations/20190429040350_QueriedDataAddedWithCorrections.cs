using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAppWithQueries.Data.Migrations
{
    public partial class QueriedDataAddedWithCorrections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MyItem",
                table: "MyItem");

            migrationBuilder.RenameTable(
                name: "MyItem",
                newName: "MyItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MyItems",
                table: "MyItems",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MyItems",
                table: "MyItems");

            migrationBuilder.RenameTable(
                name: "MyItems",
                newName: "MyItem");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MyItem",
                table: "MyItem",
                column: "Id");
        }
    }
}
