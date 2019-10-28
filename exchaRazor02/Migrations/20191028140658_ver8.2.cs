using Microsoft.EntityFrameworkCore.Migrations;

namespace exchaRazor02.Migrations
{
    public partial class ver82 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_appli",
                table: "appli");

            migrationBuilder.AlterColumn<string>(
                name: "apid",
                table: "appli",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_appli",
                table: "appli",
                columns: new[] { "diaryId", "leafTime", "apid" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_appli",
                table: "appli");

            migrationBuilder.AlterColumn<string>(
                name: "apid",
                table: "appli",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_appli",
                table: "appli",
                columns: new[] { "diaryId", "leafTime" });
        }
    }
}
