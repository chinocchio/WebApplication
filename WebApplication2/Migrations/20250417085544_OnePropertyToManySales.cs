using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class OnePropertyToManySales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SalesTransactions_PropertyId",
                table: "SalesTransactions");

            migrationBuilder.CreateIndex(
                name: "IX_SalesTransactions_PropertyId",
                table: "SalesTransactions",
                column: "PropertyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SalesTransactions_PropertyId",
                table: "SalesTransactions");

            migrationBuilder.CreateIndex(
                name: "IX_SalesTransactions_PropertyId",
                table: "SalesTransactions",
                column: "PropertyId",
                unique: true);
        }
    }
}
