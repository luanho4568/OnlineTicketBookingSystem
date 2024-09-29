using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineTicketBookingSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGroupToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameVI",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameVI",
                table: "Groups");
        }
    }
}
