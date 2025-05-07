using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class AddedNameToDocumentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "DateSubmitted",
                table: "SubmittedDocuments",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentName",
                table: "SubmittedDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocumentName",
                table: "DocumentForSubmissions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateSubmitted",
                table: "SubmittedDocuments");

            migrationBuilder.DropColumn(
                name: "DocumentName",
                table: "SubmittedDocuments");

            migrationBuilder.DropColumn(
                name: "DocumentName",
                table: "DocumentForSubmissions");
        }
    }
}
