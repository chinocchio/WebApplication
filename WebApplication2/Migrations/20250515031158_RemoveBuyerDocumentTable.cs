using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBuyerDocumentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesTransactions_BuyerDocuments_BuyerDocumentId",
                table: "SalesTransactions");

            migrationBuilder.DropTable(
                name: "BuyerDocuments");

            migrationBuilder.DropIndex(
                name: "IX_SalesTransactions_BuyerDocumentId",
                table: "SalesTransactions");

            migrationBuilder.DropColumn(
                name: "BuyerDocumentId",
                table: "SalesTransactions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BuyerDocumentId",
                table: "SalesTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BuyerDocuments",
                columns: table => new
                {
                    BuyerDocumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AIFId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalPOI = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthCertificate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cb1904Form = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cenomar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNumberOfAIF = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DOR = table.Column<DateOnly>(type: "date", nullable: true),
                    DeathCertificate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpirationDate1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpirationDate2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarriageCertificate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameOfAif = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OathOfAllegiance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pb1904Form = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PbIDType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProofOfBilling1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProofOfBilling2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SPA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SPA_ACN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecificIncomeDocsSubmitted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpsCbId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TinAuthoLetter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TinNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerifiedTin = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyerDocuments", x => x.BuyerDocumentId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesTransactions_BuyerDocumentId",
                table: "SalesTransactions",
                column: "BuyerDocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesTransactions_BuyerDocuments_BuyerDocumentId",
                table: "SalesTransactions",
                column: "BuyerDocumentId",
                principalTable: "BuyerDocuments",
                principalColumn: "BuyerDocumentId");
        }
    }
}
