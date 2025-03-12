using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_API.Migrations
{
    /// <inheritdoc />
    public partial class updte_v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentNameSnapshot",
                table: "EmployeeDepartmentHistories");

            migrationBuilder.DropColumn(
                name: "PositionNameSnapshot",
                table: "EmployeeDepartmentHistories");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "Username");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "Email");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentNameSnapshot",
                table: "EmployeeDepartmentHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PositionNameSnapshot",
                table: "EmployeeDepartmentHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
