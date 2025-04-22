using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class SellerV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesTransactions_Properties_PropertyId",
                table: "SalesTransactions");

            migrationBuilder.AlterColumn<int>(
                name: "PropertyId",
                table: "SalesTransactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "SalesProponentsId",
                table: "SalesTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SalesProponents",
                columns: table => new
                {
                    SalesProponentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Broker = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PS_QC_ISM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarketingOfficer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarketingManager = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeputyMarketingDirector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarketingDirector = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesProponents", x => x.SalesProponentId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesTransactions_SalesProponentsId",
                table: "SalesTransactions",
                column: "SalesProponentsId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesTransactions_Properties_PropertyId",
                table: "SalesTransactions",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "PropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesTransactions_SalesProponents_SalesProponentsId",
                table: "SalesTransactions",
                column: "SalesProponentsId",
                principalTable: "SalesProponents",
                principalColumn: "SalesProponentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesTransactions_Properties_PropertyId",
                table: "SalesTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesTransactions_SalesProponents_SalesProponentsId",
                table: "SalesTransactions");

            migrationBuilder.DropTable(
                name: "SalesProponents");

            migrationBuilder.DropIndex(
                name: "IX_SalesTransactions_SalesProponentsId",
                table: "SalesTransactions");

            migrationBuilder.DropColumn(
                name: "SalesProponentsId",
                table: "SalesTransactions");

            migrationBuilder.AlterColumn<int>(
                name: "PropertyId",
                table: "SalesTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesTransactions_Properties_PropertyId",
                table: "SalesTransactions",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "PropertyId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
