using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class BuyerLedgerAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BuyerLedgers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractNumber = table.Column<long>(type: "bigint", nullable: true),
                    CustomerCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentTermSchedule = table.Column<DateOnly>(type: "date", nullable: true),
                    PaymentNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmountDue = table.Column<double>(type: "float", nullable: true),
                    WhenDue = table.Column<DateOnly>(type: "date", nullable: true),
                    PaymentReferenceDocNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentReferenceDocType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentReferenceAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentReferenceDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyerLedgers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuyerLedgers");
        }
    }
}
