using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class RfTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReservationFeeId",
                table: "SalesTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReservationFees",
                columns: table => new
                {
                    ReservationFeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RfDatePaid = table.Column<DateOnly>(type: "date", nullable: false),
                    RfProofUsedUponHolding = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RfAmountPaidToUnit = table.Column<double>(type: "float", nullable: false),
                    RfAmountPaidToGMTOE = table.Column<double>(type: "float", nullable: false),
                    RfAmountGMTOE_Unit = table.Column<double>(type: "float", nullable: false),
                    RfDateCredited = table.Column<DateOnly>(type: "date", nullable: false),
                    RfOrNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationFees", x => x.ReservationFeeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesTransactions_ReservationFeeId",
                table: "SalesTransactions",
                column: "ReservationFeeId",
                unique: true,
                filter: "[ReservationFeeId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesTransactions_ReservationFees_ReservationFeeId",
                table: "SalesTransactions",
                column: "ReservationFeeId",
                principalTable: "ReservationFees",
                principalColumn: "ReservationFeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesTransactions_ReservationFees_ReservationFeeId",
                table: "SalesTransactions");

            migrationBuilder.DropTable(
                name: "ReservationFees");

            migrationBuilder.DropIndex(
                name: "IX_SalesTransactions_ReservationFeeId",
                table: "SalesTransactions");

            migrationBuilder.DropColumn(
                name: "ReservationFeeId",
                table: "SalesTransactions");
        }
    }
}
