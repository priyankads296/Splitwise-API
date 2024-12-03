using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Splitwise.Migrations
{
    /// <inheritdoc />
    public partial class GroupUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_GroupDetails_GroupDetailsId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ExpenseDetail");

            migrationBuilder.RenameColumn(
                name: "GroupDetailsId",
                table: "Groups",
                newName: "GroupDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_GroupDetailsId",
                table: "Groups",
                newName: "IX_Groups_GroupDetailId");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Expenses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_GroupDetails_GroupDetailId",
                table: "Groups",
                column: "GroupDetailId",
                principalTable: "GroupDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_GroupDetails_GroupDetailId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Expenses");

            migrationBuilder.RenameColumn(
                name: "GroupDetailId",
                table: "Groups",
                newName: "GroupDetailsId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_GroupDetailId",
                table: "Groups",
                newName: "IX_Groups_GroupDetailsId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ExpenseDetail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_GroupDetails_GroupDetailsId",
                table: "Groups",
                column: "GroupDetailsId",
                principalTable: "GroupDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
