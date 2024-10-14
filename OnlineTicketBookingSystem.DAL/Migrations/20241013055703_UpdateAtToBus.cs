using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineTicketBookingSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAtToBus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UpadtedAt",
                table: "Buses",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpadtedAt",
                table: "Buses");
        }
    }
}
