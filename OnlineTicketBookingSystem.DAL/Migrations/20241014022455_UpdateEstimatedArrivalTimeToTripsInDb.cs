using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineTicketBookingSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEstimatedArrivalTimeToTripsInDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EstimatedArrivalTime",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimatedArrivalTime",
                table: "Trips");
        }
    }
}
