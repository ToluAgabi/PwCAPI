using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PwCAssessment.Migrations
{
    public partial class ReqNo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RequisitionNumber",
                table: "TravelRequests",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_TravelRequests_RequisitionNumber",
                table: "TravelRequests",
                column: "RequisitionNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TravelRequests_RequisitionNumber",
                table: "TravelRequests");

            migrationBuilder.AlterColumn<string>(
                name: "RequisitionNumber",
                table: "TravelRequests",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
