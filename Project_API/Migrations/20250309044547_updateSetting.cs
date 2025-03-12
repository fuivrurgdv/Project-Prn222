using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project_API.Migrations
{
    /// <inheritdoc />
    public partial class updateSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    AttendanceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClockIn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClockOut = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ScheduledClockIn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScheduledClockOut = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalHours = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OTHours = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LateReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LateApprovalStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LateApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EarlyLeaveReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EarlyLeaveStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EarlyLeaveApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.AttendanceID);
                    table.ForeignKey(
                        name: "FK_Attendances_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShiftSettings",
                columns: table => new
                {
                    ShiftSettingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClockInTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ClockOutTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreateDay = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftSettings", x => x.ShiftSettingID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_UserID",
                table: "Attendances",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "ShiftSettings");
        }
    }
}
