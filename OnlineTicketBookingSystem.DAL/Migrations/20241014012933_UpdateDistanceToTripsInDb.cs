using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineTicketBookingSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDistanceToTripsInDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Distance",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Distance",
                table: "Trips");
        }
    }
}
