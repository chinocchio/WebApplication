using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class BuyerAndSalesDocumentAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RfProofUsedUponHolding",
                table: "ReservationFees");

            migrationBuilder.AddColumn<int>(
                name: "BuyerDocumentId",
                table: "SalesTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SalesDocumentId",
                table: "SalesTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstHomeInPH",
                table: "CreditReviews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HighRishk",
                table: "CreditReviews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HighRiskFactors",
                table: "CreditReviews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImmigrantOrNonImmigrant",
                table: "CreditReviews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IncomeDeclaredCb",
                table: "CreditReviews",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IncomeDeclaredPb",
                table: "CreditReviews",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfHomesInPH",
                table: "CreditReviews",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonaCategory",
                table: "CreditReviews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectOrUnitCodeOfOtherCPGIUnit",
                table: "CreditReviews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReasonForPurchase",
                table: "CreditReviews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalIncomeCombined",
                table: "CreditReviews",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WithOtherCPGIUnits",
                table: "CreditReviews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BuyerDocuments",
                columns: table => new
                {
                    BuyerDocumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SPA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SPA_ACN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DOR = table.Column<DateOnly>(type: "date", nullable: false),
                    BirthCertificate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cenomar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarriageCertificate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeathCertificate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProofOfBilling1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerifiedTin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TinAuthoLetter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pb1904Form = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cb1904Form = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TinNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PbIDType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpirationDate1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpsCbId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpirationDate2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecificIncomeDocsSubmitted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AIFId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameOfAif = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNumberOfAIF = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProofOfBilling2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalPOI = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OathOfAllegiance = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyerDocuments", x => x.BuyerDocumentId);
                });

            migrationBuilder.CreateTable(
                name: "SalesDocuments",
                columns: table => new
                {
                    SalesDocumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RfProofUsedUponHolding = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sa1Sa2CompletionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ProofOfIncomeSubmitted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocsSubmissionForCiEndorsement = table.Column<DateOnly>(type: "date", nullable: false),
                    TotalPDCSRequired = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankPDCIssued = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalAdaPdcsSubmitted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateSubmitted = table.Column<DateOnly>(type: "date", nullable: false),
                    PDCCompletionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CTSToDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CTSStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeOfCTS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignedByBroker = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CtsACN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateRecievedFromSalesAdmin = table.Column<DateOnly>(type: "date", nullable: false),
                    DateNotary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SetBDocsCompletionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ContractedSaleDate = table.Column<DateOnly>(type: "date", nullable: false),
                    MonthContracted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MonthBooked = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    P1OldColorStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AegingCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LackingDocs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CtsSpaAndDOROutDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ComplianceDUe = table.Column<DateOnly>(type: "date", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesDocuments", x => x.SalesDocumentId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesTransactions_BuyerDocumentId",
                table: "SalesTransactions",
                column: "BuyerDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesTransactions_SalesDocumentId",
                table: "SalesTransactions",
                column: "SalesDocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesTransactions_BuyerDocuments_BuyerDocumentId",
                table: "SalesTransactions",
                column: "BuyerDocumentId",
                principalTable: "BuyerDocuments",
                principalColumn: "BuyerDocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesTransactions_SalesDocuments_SalesDocumentId",
                table: "SalesTransactions",
                column: "SalesDocumentId",
                principalTable: "SalesDocuments",
                principalColumn: "SalesDocumentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesTransactions_BuyerDocuments_BuyerDocumentId",
                table: "SalesTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesTransactions_SalesDocuments_SalesDocumentId",
                table: "SalesTransactions");

            migrationBuilder.DropTable(
                name: "BuyerDocuments");

            migrationBuilder.DropTable(
                name: "SalesDocuments");

            migrationBuilder.DropIndex(
                name: "IX_SalesTransactions_BuyerDocumentId",
                table: "SalesTransactions");

            migrationBuilder.DropIndex(
                name: "IX_SalesTransactions_SalesDocumentId",
                table: "SalesTransactions");

            migrationBuilder.DropColumn(
                name: "BuyerDocumentId",
                table: "SalesTransactions");

            migrationBuilder.DropColumn(
                name: "SalesDocumentId",
                table: "SalesTransactions");

            migrationBuilder.DropColumn(
                name: "FirstHomeInPH",
                table: "CreditReviews");

            migrationBuilder.DropColumn(
                name: "HighRishk",
                table: "CreditReviews");

            migrationBuilder.DropColumn(
                name: "HighRiskFactors",
                table: "CreditReviews");

            migrationBuilder.DropColumn(
                name: "ImmigrantOrNonImmigrant",
                table: "CreditReviews");

            migrationBuilder.DropColumn(
                name: "IncomeDeclaredCb",
                table: "CreditReviews");

            migrationBuilder.DropColumn(
                name: "IncomeDeclaredPb",
                table: "CreditReviews");

            migrationBuilder.DropColumn(
                name: "NumberOfHomesInPH",
                table: "CreditReviews");

            migrationBuilder.DropColumn(
                name: "PersonaCategory",
                table: "CreditReviews");

            migrationBuilder.DropColumn(
                name: "ProjectOrUnitCodeOfOtherCPGIUnit",
                table: "CreditReviews");

            migrationBuilder.DropColumn(
                name: "ReasonForPurchase",
                table: "CreditReviews");

            migrationBuilder.DropColumn(
                name: "TotalIncomeCombined",
                table: "CreditReviews");

            migrationBuilder.DropColumn(
                name: "WithOtherCPGIUnits",
                table: "CreditReviews");

            migrationBuilder.AddColumn<string>(
                name: "RfProofUsedUponHolding",
                table: "ReservationFees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
