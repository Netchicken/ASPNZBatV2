using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ASPNZBat.Migrations
{
    public partial class seatdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SeatDateEnd",
                table: "SeatBooking",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeatDateEnd",
                table: "SeatBooking");
        }
    }
}
