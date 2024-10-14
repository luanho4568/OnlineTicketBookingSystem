using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineTicketBookingSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAtToBus1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpadtedAt",
                table: "Buses",
                newName: "UpdatedAt");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Buses",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Buses",
                newName: "UpadtedAt");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Buses",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
