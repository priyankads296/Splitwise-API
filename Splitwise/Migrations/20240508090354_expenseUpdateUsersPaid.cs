using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Splitwise.Migrations
{
    /// <inheritdoc />
    public partial class expenseUpdateUsersPaid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExpenseId1",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ExpenseId1",
                table: "Users",
                column: "ExpenseId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Expenses_ExpenseId1",
                table: "Users",
                column: "ExpenseId1",
                principalTable: "Expenses",
                principalColumn: "ExpenseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Expenses_ExpenseId1",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ExpenseId1",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ExpenseId1",
                table: "Users");
        }
    }
}
