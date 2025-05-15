using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class RemodeldSalesProponent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesTransactions_SalesProponents_SalesProponentsId",
                table: "SalesTransactions");

            migrationBuilder.DropIndex(
                name: "IX_SalesTransactions_SalesProponentsId",
                table: "SalesTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SalesProponents",
                table: "SalesProponents");

            migrationBuilder.DropColumn(
                name: "SalesProponentsId",
                table: "SalesTransactions");

            migrationBuilder.DropColumn(
                name: "SalesProponentId",
                table: "SalesProponents");

            migrationBuilder.DropColumn(
                name: "Broker",
                table: "SalesProponents");

            migrationBuilder.DropColumn(
                name: "DeputyMarketingDirector",
                table: "SalesProponents");

            migrationBuilder.DropColumn(
                name: "MarketingDirector",
                table: "SalesProponents");

            migrationBuilder.RenameColumn(
                name: "PS_QC_ISM",
                table: "SalesProponents",
                newName: "Roles");

            migrationBuilder.RenameColumn(
                name: "MarketingOfficer",
                table: "SalesProponents",
                newName: "ReportingTo");

            migrationBuilder.RenameColumn(
                name: "MarketingManager",
                table: "SalesProponents",
                newName: "Fullname");

            migrationBuilder.AddColumn<long>(
                name: "ProponentBpNumber",
                table: "SalesTransactions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ProponentBpNumber",
                table: "SalesProponents",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProponentBpNumber",
                table: "SalesTransactions");

            migrationBuilder.DropColumn(
                name: "ProponentBpNumber",
                table: "SalesProponents");

            migrationBuilder.RenameColumn(
                name: "Roles",
                table: "SalesProponents",
                newName: "PS_QC_ISM");

            migrationBuilder.RenameColumn(
                name: "ReportingTo",
                table: "SalesProponents",
                newName: "MarketingOfficer");

            migrationBuilder.RenameColumn(
                name: "Fullname",
                table: "SalesProponents",
                newName: "MarketingManager");

            migrationBuilder.AddColumn<int>(
                name: "SalesProponentsId",
                table: "SalesTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SalesProponentId",
                table: "SalesProponents",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Broker",
                table: "SalesProponents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeputyMarketingDirector",
                table: "SalesProponents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarketingDirector",
                table: "SalesProponents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalesProponents",
                table: "SalesProponents",
                column: "SalesProponentId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesTransactions_SalesProponentsId",
                table: "SalesTransactions",
                column: "SalesProponentsId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesTransactions_SalesProponents_SalesProponentsId",
                table: "SalesTransactions",
                column: "SalesProponentsId",
                principalTable: "SalesProponents",
                principalColumn: "SalesProponentId");
        }
    }
}
