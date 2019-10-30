using Microsoft.EntityFrameworkCore.Migrations;

namespace exchaRazor02.Migrations
{
    public partial class double2int : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "period",
                table: "appli",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "period",
                table: "appli",
                type: "float",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
