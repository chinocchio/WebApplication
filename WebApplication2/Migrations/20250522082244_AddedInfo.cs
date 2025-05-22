using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class AddedInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateSubmitted",
                table: "SubmittedDocuments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Citizenship",
                table: "BusinessPartners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "BusinessPartners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DemographicByMarket",
                table: "BusinessPartners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmploymentCategory",
                table: "BusinessPartners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmploymentCountry",
                table: "BusinessPartners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmploymentRegion",
                table: "BusinessPartners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmploymentState",
                table: "BusinessPartners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IndustryType",
                table: "BusinessPartners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "BusinessPartners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "BusinessPartners",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Citizenship",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "DemographicByMarket",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "EmploymentCategory",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "EmploymentCountry",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "EmploymentRegion",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "EmploymentState",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "IndustryType",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "BusinessPartners");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "BusinessPartners");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DateSubmitted",
                table: "SubmittedDocuments",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
