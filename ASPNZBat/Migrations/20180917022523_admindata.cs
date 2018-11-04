using Microsoft.EntityFrameworkCore.Migrations;

namespace ASPNZBat.Migrations
{
    public partial class admindata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.CreateTable(
                name: "AdminData",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    isVisibleS1L = table.Column<bool>(nullable: false),
                    isVisibleS2L = table.Column<bool>(nullable: false),
                    isVisibleS3L = table.Column<bool>(nullable: false),
                    isVisibleS4L = table.Column<bool>(nullable: false),
                    isVisibleS5L = table.Column<bool>(nullable: false),
                    isVisibleS6L = table.Column<bool>(nullable: false),
                    isVisibleS7L = table.Column<bool>(nullable: false),
                    isVisibleS8L = table.Column<bool>(nullable: false),
                    isVisibleS9L = table.Column<bool>(nullable: false),
                    isVisibleS10L = table.Column<bool>(nullable: false),
                    isVisibleS11L = table.Column<bool>(nullable: false),
                    isVisibleS12L = table.Column<bool>(nullable: false),
                    isVisibleS13L = table.Column<bool>(nullable: false),
                    isVisibleS14L = table.Column<bool>(nullable: false),
                    isVisibleS15L = table.Column<bool>(nullable: false),
                    isVisibleS16L = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminData", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminData");


        }
    }
}
