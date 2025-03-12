using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_API.Migrations
{
    /// <inheritdoc />
    public partial class updte_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalaryLevel_Users_UserID",
                table: "SalaryLevel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SalaryLevel",
                table: "SalaryLevel");

            migrationBuilder.DropIndex(
                name: "IX_SalaryLevel_UserID",
                table: "SalaryLevel");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "SalaryLevel");

            migrationBuilder.RenameTable(
                name: "SalaryLevel",
                newName: "SalaryLevels");

            migrationBuilder.RenameColumn(
                name: "SalaryLevelID",
                table: "SalaryLevels",
                newName: "SalaryLevelId");

            migrationBuilder.AddColumn<int>(
                name: "SalaryLevelId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<double>(
                name: "BasicSalary",
                table: "SalaryLevels",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalaryLevels",
                table: "SalaryLevels",
                column: "SalaryLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_SalaryLevelId",
                table: "Users",
                column: "SalaryLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_SalaryLevels_SalaryLevelId",
                table: "Users",
                column: "SalaryLevelId",
                principalTable: "SalaryLevels",
                principalColumn: "SalaryLevelId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_SalaryLevels_SalaryLevelId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_SalaryLevelId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SalaryLevels",
                table: "SalaryLevels");

            migrationBuilder.DropColumn(
                name: "SalaryLevelId",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "SalaryLevels",
                newName: "SalaryLevel");

            migrationBuilder.RenameColumn(
                name: "SalaryLevelId",
                table: "SalaryLevel",
                newName: "SalaryLevelID");

            migrationBuilder.AlterColumn<decimal>(
                name: "BasicSalary",
                table: "SalaryLevel",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "SalaryLevel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalaryLevel",
                table: "SalaryLevel",
                column: "SalaryLevelID");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryLevel_UserID",
                table: "SalaryLevel",
                column: "UserID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SalaryLevel_Users_UserID",
                table: "SalaryLevel",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
