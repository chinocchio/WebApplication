using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class DocumentTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentForSubmissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentFinanceType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentSourceOfIncome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentGroup = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentForSubmissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentRegistries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoesExprire = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DurationInMonths = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentRegistries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubmittedDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractNumber = table.Column<long>(type: "bigint", nullable: true),
                    CustomerCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubmittedDocuments", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentForSubmissions");

            migrationBuilder.DropTable(
                name: "DocumentRegistries");

            migrationBuilder.DropTable(
                name: "SubmittedDocuments");
        }
    }
}
