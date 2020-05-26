using Microsoft.EntityFrameworkCore.Migrations;

namespace SiteMap.Migrations
{
    public partial class Mig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "URLs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_URLs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SiteMapUrls",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteMapUrlString = table.Column<string>(nullable: true),
                    AccessMS = table.Column<double>(nullable: false),
                    URLID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteMapUrls", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SiteMapUrls_URLs_URLID",
                        column: x => x.URLID,
                        principalTable: "URLs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SiteMapUrls_URLID",
                table: "SiteMapUrls",
                column: "URLID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SiteMapUrls");

            migrationBuilder.DropTable(
                name: "URLs");
        }
    }
}
