using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class AddedSomeTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "HoldingDate",
                table: "SalesTransactions",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "PromoDiscount",
                table: "SalesTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionType",
                table: "SalesTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Milestone",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewColorStatus",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusInGeneral",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientBase",
                table: "BusinessPartners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactNumber",
                table: "BusinessPartners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "BusinessPartners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "IdDateSubmitted",
                table: "BusinessPartners",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdSubmitted",
                table: "BusinessPartners",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoldingDate",
                table: "SalesTransactions");

            migrationBuilder.DropColumn(
                name: "PromoDiscount",
                table: "SalesTransactions");

            migrationBuilder.DropColumn(
                name: "TransactionType",
                table: "SalesTransactions");

            migrationBuilder.DropColumn(
                name: "Milestone",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "NewColorStatus",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "StatusInGeneral",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "ClientBase",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "ContactNumber",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "IdDateSubmitted",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "IdSubmitted",
                table: "BusinessPartners");
        }
    }
}
