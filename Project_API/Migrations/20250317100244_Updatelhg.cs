using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_API.Migrations
{
    /// <inheritdoc />
    public partial class Updatelhg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "LeaveRequests",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reason",
                table: "LeaveRequests");
        }
    }
}
