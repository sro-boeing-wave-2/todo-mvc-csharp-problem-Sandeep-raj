using Microsoft.EntityFrameworkCore.Migrations;

namespace GoogleKeepNotes.Migrations
{
    public partial class AddedIsChecked : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isChecked",
                table: "CheckList",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isChecked",
                table: "CheckList");
        }
    }
}
