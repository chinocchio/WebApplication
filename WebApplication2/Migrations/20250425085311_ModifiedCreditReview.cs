using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedCreditReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessPartners_CreditReviews_CreditReviewId",
                table: "BusinessPartners");

            migrationBuilder.DropIndex(
                name: "IX_BusinessPartners_CreditReviewId",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "CreditReviewId",
                table: "BusinessPartners");

            migrationBuilder.AddColumn<int>(
                name: "CreditReviewId",
                table: "SalesTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesTransactions_CreditReviewId",
                table: "SalesTransactions",
                column: "CreditReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesTransactions_CreditReviews_CreditReviewId",
                table: "SalesTransactions",
                column: "CreditReviewId",
                principalTable: "CreditReviews",
                principalColumn: "CreditReviewId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesTransactions_CreditReviews_CreditReviewId",
                table: "SalesTransactions");

            migrationBuilder.DropIndex(
                name: "IX_SalesTransactions_CreditReviewId",
                table: "SalesTransactions");

            migrationBuilder.DropColumn(
                name: "CreditReviewId",
                table: "SalesTransactions");

            migrationBuilder.AddColumn<int>(
                name: "CreditReviewId",
                table: "BusinessPartners",
                type: "int",
                nullable: true);

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
    }
}
