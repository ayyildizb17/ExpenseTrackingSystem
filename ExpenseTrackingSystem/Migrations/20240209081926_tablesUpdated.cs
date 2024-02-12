using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTrackingSystem.Migrations
{
    /// <inheritdoc />
    public partial class tablesUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Expense",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ManagerId",
                table: "Expense",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "Expense",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_ManagerId",
                table: "Expense",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_AspNetUsers_ManagerId",
                table: "Expense",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expense_AspNetUsers_ManagerId",
                table: "Expense");

            migrationBuilder.DropIndex(
                name: "IX_Expense_ManagerId",
                table: "Expense");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Expense");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Expense");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Expense");
        }
    }
}
