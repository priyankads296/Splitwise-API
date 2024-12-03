using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Splitwise.Migrations
{
    /// <inheritdoc />
    public partial class updateExpenseTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseDetail_Expenses_ExpenseId",
                table: "ExpenseDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpenseDetail",
                table: "ExpenseDetail");

            migrationBuilder.RenameTable(
                name: "ExpenseDetail",
                newName: "ExpenseDetails");

            migrationBuilder.RenameIndex(
                name: "IX_ExpenseDetail_ExpenseId",
                table: "ExpenseDetails",
                newName: "IX_ExpenseDetails_ExpenseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpenseDetails",
                table: "ExpenseDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseDetails_Expenses_ExpenseId",
                table: "ExpenseDetails",
                column: "ExpenseId",
                principalTable: "Expenses",
                principalColumn: "ExpenseId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseDetails_Expenses_ExpenseId",
                table: "ExpenseDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpenseDetails",
                table: "ExpenseDetails");

            migrationBuilder.RenameTable(
                name: "ExpenseDetails",
                newName: "ExpenseDetail");

            migrationBuilder.RenameIndex(
                name: "IX_ExpenseDetails_ExpenseId",
                table: "ExpenseDetail",
                newName: "IX_ExpenseDetail_ExpenseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpenseDetail",
                table: "ExpenseDetail",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseDetail_Expenses_ExpenseId",
                table: "ExpenseDetail",
                column: "ExpenseId",
                principalTable: "Expenses",
                principalColumn: "ExpenseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
