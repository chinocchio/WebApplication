using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class BusinessPartnerEntityAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BusinessPartnerId",
                table: "SalesTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BusinessPartners",
                columns: table => new
                {
                    BusinessPartnerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fullname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerCode = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessPartners", x => x.BusinessPartnerId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesTransactions_BusinessPartnerId",
                table: "SalesTransactions",
                column: "BusinessPartnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesTransactions_BusinessPartners_BusinessPartnerId",
                table: "SalesTransactions",
                column: "BusinessPartnerId",
                principalTable: "BusinessPartners",
                principalColumn: "BusinessPartnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesTransactions_BusinessPartners_BusinessPartnerId",
                table: "SalesTransactions");

            migrationBuilder.DropTable(
                name: "BusinessPartners");

            migrationBuilder.DropIndex(
                name: "IX_SalesTransactions_BusinessPartnerId",
                table: "SalesTransactions");

            migrationBuilder.DropColumn(
                name: "BusinessPartnerId",
                table: "SalesTransactions");
        }
    }
}
