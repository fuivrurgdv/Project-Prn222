using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_API.Migrations
{
    /// <inheritdoc />
    public partial class updaterfrfrf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notification",
                table: "Attendances",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notification",
                table: "Attendances");
        }
    }
}
