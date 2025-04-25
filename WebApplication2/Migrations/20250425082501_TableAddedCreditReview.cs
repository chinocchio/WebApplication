using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class TableAddedCreditReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UnitCode",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PropertyType",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProjectName",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BuildingPhase",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreditReviewId",
                table: "BusinessPartners",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CreditReviews",
                columns: table => new
                {
                    CreditReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreditReviewResult = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CMAPResult = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreditReviewRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NDIStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsBankApporvable = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RedTag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RedTagReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CiCompletionDate = table.Column<DateOnly>(type: "date", nullable: false),
                    TypeOfIncome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NdiRate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NetDisposableIncome = table.Column<int>(type: "int", nullable: false),
                    OtherLoans = table.Column<int>(type: "int", nullable: false),
                    NetNdi = table.Column<int>(type: "int", nullable: false),
                    NdiVsBankMaTobAmt = table.Column<int>(type: "int", nullable: false),
                    PercentOfNdiVsMa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NdiCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxTerm = table.Column<int>(type: "int", nullable: false),
                    EstimatedMaxTerm = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditReviews", x => x.CreditReviewId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPartners_CreditReviewId",
                table: "BusinessPartners",
                column: "CreditReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessPartners_CreditReviews_CreditReviewId",
                table: "BusinessPartners",
                column: "CreditReviewId",
                principalTable: "CreditReviews",
                principalColumn: "CreditReviewId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessPartners_CreditReviews_CreditReviewId",
                table: "BusinessPartners");

            migrationBuilder.DropTable(
                name: "CreditReviews");

            migrationBuilder.DropIndex(
                name: "IX_BusinessPartners_CreditReviewId",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "CreditReviewId",
                table: "BusinessPartners");

            migrationBuilder.AlterColumn<string>(
                name: "UnitCode",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PropertyType",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ProjectName",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "BuildingPhase",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
