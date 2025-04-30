using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class ResevationFeeToPaymentTerm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesTransactions_ReservationFees_ReservationFeeId",
                table: "SalesTransactions");

            migrationBuilder.DropTable(
                name: "ReservationFees");

            migrationBuilder.DropIndex(
                name: "IX_SalesTransactions_ReservationFeeId",
                table: "SalesTransactions");

            migrationBuilder.RenameColumn(
                name: "ReservationFeeId",
                table: "SalesTransactions",
                newName: "PaymentTermId");

            migrationBuilder.CreateTable(
                name: "PaymentTerms",
                columns: table => new
                {
                    PaymentTermId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RfDatePaid = table.Column<DateOnly>(type: "date", nullable: true),
                    RfAmountPaidToUnit = table.Column<double>(type: "float", nullable: true),
                    RfAmountPaidToGMTOE = table.Column<double>(type: "float", nullable: true),
                    RfAmountGMTOE_Unit = table.Column<double>(type: "float", nullable: true),
                    RfDateCredited = table.Column<DateOnly>(type: "date", nullable: true),
                    RfOrNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Paymentterm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PercentTOB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TOBModeOfPayment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TOB = table.Column<double>(type: "float", nullable: true),
                    EstimatedBankMAFor7Point5Percent = table.Column<double>(type: "float", nullable: true),
                    StartDate1stMA = table.Column<DateOnly>(type: "date", nullable: true),
                    UnitParking = table.Column<double>(type: "float", nullable: true),
                    TFee = table.Column<double>(type: "float", nullable: true),
                    AmountPaidToUnit = table.Column<double>(type: "float", nullable: true),
                    AmountPaidToTF = table.Column<double>(type: "float", nullable: true),
                    DatePaid = table.Column<DateOnly>(type: "date", nullable: true),
                    FirstMAOrNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentDueDate = table.Column<int>(type: "int", nullable: true),
                    Unit = table.Column<double>(type: "float", nullable: true),
                    TransferFee = table.Column<double>(type: "float", nullable: true),
                    Total = table.Column<double>(type: "float", nullable: true),
                    PercentPayment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentCategory = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTerms", x => x.PaymentTermId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesTransactions_PaymentTermId",
                table: "SalesTransactions",
                column: "PaymentTermId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesTransactions_PaymentTerms_PaymentTermId",
                table: "SalesTransactions",
                column: "PaymentTermId",
                principalTable: "PaymentTerms",
                principalColumn: "PaymentTermId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesTransactions_PaymentTerms_PaymentTermId",
                table: "SalesTransactions");

            migrationBuilder.DropTable(
                name: "PaymentTerms");

            migrationBuilder.DropIndex(
                name: "IX_SalesTransactions_PaymentTermId",
                table: "SalesTransactions");

            migrationBuilder.RenameColumn(
                name: "PaymentTermId",
                table: "SalesTransactions",
                newName: "ReservationFeeId");

            migrationBuilder.CreateTable(
                name: "ReservationFees",
                columns: table => new
                {
                    ReservationFeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RfAmountGMTOE_Unit = table.Column<double>(type: "float", nullable: false),
                    RfAmountPaidToGMTOE = table.Column<double>(type: "float", nullable: false),
                    RfAmountPaidToUnit = table.Column<double>(type: "float", nullable: false),
                    RfDateCredited = table.Column<DateOnly>(type: "date", nullable: false),
                    RfDatePaid = table.Column<DateOnly>(type: "date", nullable: false),
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
    }
}
