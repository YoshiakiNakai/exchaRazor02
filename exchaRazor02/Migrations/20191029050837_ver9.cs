using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace exchaRazor02.Migrations
{
    public partial class ver9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "diaries",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 255, nullable: false),
                    pass = table.Column<string>(nullable: false),
                    note = table.Column<string>(maxLength: 255, nullable: true),
                    last = table.Column<DateTime>(nullable: false),
                    pub = table.Column<int>(nullable: false),
                    excha = table.Column<int>(nullable: false),
                    writa = table.Column<int>(nullable: false),
                    retTime = table.Column<DateTime>(nullable: false),
                    exid = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_diaries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "leaves",
                columns: table => new
                {
                    diaryId = table.Column<string>(nullable: false),
                    time = table.Column<DateTime>(nullable: false),
                    title = table.Column<string>(maxLength: 255, nullable: true),
                    contents = table.Column<string>(maxLength: 65535, nullable: true),
                    exid = table.Column<string>(nullable: true),
                    comment = table.Column<string>(maxLength: 65535, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_leaves", x => new { x.diaryId, x.time });
                    table.ForeignKey(
                        name: "FK_leaves_diaries_diaryId",
                        column: x => x.diaryId,
                        principalTable: "diaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "appli",
                columns: table => new
                {
                    diaryId = table.Column<string>(nullable: false),
                    leafTime = table.Column<DateTime>(nullable: false),
                    apid = table.Column<string>(nullable: false),
                    accept = table.Column<int>(nullable: false),
                    period = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appli", x => new { x.diaryId, x.leafTime, x.apid });
                    table.ForeignKey(
                        name: "FK_appli_leaves_diaryId_leafTime",
                        columns: x => new { x.diaryId, x.leafTime },
                        principalTable: "leaves",
                        principalColumns: new[] { "diaryId", "time" },
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "appli");

            migrationBuilder.DropTable(
                name: "leaves");

            migrationBuilder.DropTable(
                name: "diaries");
        }
    }
}
