using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class LipatColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Milestone",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "NewColorStatus",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "StatusInGeneral",
                table: "Properties");

            migrationBuilder.AddColumn<string>(
                name: "Milestone",
                table: "SalesTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewColorStatus",
                table: "SalesTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusInGeneral",
                table: "SalesTransactions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Milestone",
                table: "SalesTransactions");

            migrationBuilder.DropColumn(
                name: "NewColorStatus",
                table: "SalesTransactions");

            migrationBuilder.DropColumn(
                name: "StatusInGeneral",
                table: "SalesTransactions");

            migrationBuilder.AddColumn<string>(
                name: "Milestone",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewColorStatus",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusInGeneral",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
