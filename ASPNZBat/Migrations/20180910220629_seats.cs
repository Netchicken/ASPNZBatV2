using Microsoft.EntityFrameworkCore.Migrations;

namespace ASPNZBat.Migrations
{
    public partial class seats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "S1",
                table: "SeatBooking",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "S10",
                table: "SeatBooking",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "S11",
                table: "SeatBooking",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "S12",
                table: "SeatBooking",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "S13",
                table: "SeatBooking",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "S14",
                table: "SeatBooking",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "S15",
                table: "SeatBooking",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "S16",
                table: "SeatBooking",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "S2",
                table: "SeatBooking",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "S3",
                table: "SeatBooking",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "S4",
                table: "SeatBooking",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "S5",
                table: "SeatBooking",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "S6",
                table: "SeatBooking",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "S7",
                table: "SeatBooking",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "S8",
                table: "SeatBooking",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "S9",
                table: "SeatBooking",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "S1",
                table: "SeatBooking");

            migrationBuilder.DropColumn(
                name: "S10",
                table: "SeatBooking");

            migrationBuilder.DropColumn(
                name: "S11",
                table: "SeatBooking");

            migrationBuilder.DropColumn(
                name: "S12",
                table: "SeatBooking");

            migrationBuilder.DropColumn(
                name: "S13",
                table: "SeatBooking");

            migrationBuilder.DropColumn(
                name: "S14",
                table: "SeatBooking");

            migrationBuilder.DropColumn(
                name: "S15",
                table: "SeatBooking");

            migrationBuilder.DropColumn(
                name: "S16",
                table: "SeatBooking");

            migrationBuilder.DropColumn(
                name: "S2",
                table: "SeatBooking");

            migrationBuilder.DropColumn(
                name: "S3",
                table: "SeatBooking");

            migrationBuilder.DropColumn(
                name: "S4",
                table: "SeatBooking");

            migrationBuilder.DropColumn(
                name: "S5",
                table: "SeatBooking");

            migrationBuilder.DropColumn(
                name: "S6",
                table: "SeatBooking");

            migrationBuilder.DropColumn(
                name: "S7",
                table: "SeatBooking");

            migrationBuilder.DropColumn(
                name: "S8",
                table: "SeatBooking");

            migrationBuilder.DropColumn(
                name: "S9",
                table: "SeatBooking");
        }
    }
}
