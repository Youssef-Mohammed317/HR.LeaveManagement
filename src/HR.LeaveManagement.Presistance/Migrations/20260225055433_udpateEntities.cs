using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR.LeaveManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class udpateEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cancelled",
                table: "LeaveRequests");

            migrationBuilder.RenameColumn(
                name: "RequestingEmployeeId",
                table: "LeaveRequests",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_LeaveRequests_RequestingEmployeeId",
                table: "LeaveRequests",
                newName: "IX_LeaveRequests_EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "LeaveRequests",
                newName: "RequestingEmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_LeaveRequests_EmployeeId",
                table: "LeaveRequests",
                newName: "IX_LeaveRequests_RequestingEmployeeId");

            migrationBuilder.AddColumn<bool>(
                name: "Cancelled",
                table: "LeaveRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
