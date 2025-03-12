using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_API.Migrations
{
    /// <inheritdoc />
    public partial class update_tưeptheo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LateApprovalDate",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "LateApprovalStatus",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "OTHours",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "TotalHours",
                table: "Attendances");

            migrationBuilder.RenameColumn(
                name: "LateReason",
                table: "Attendances",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Attendances",
                newName: "LateReason");

            migrationBuilder.AddColumn<DateTime>(
                name: "LateApprovalDate",
                table: "Attendances",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LateApprovalStatus",
                table: "Attendances",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OTHours",
                table: "Attendances",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalHours",
                table: "Attendances",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
