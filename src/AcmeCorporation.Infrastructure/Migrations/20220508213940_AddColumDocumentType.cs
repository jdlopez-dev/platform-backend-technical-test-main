using Microsoft.EntityFrameworkCore.Migrations;

namespace AcmeCorporation.Infrastructure.Migrations
{
    public partial class AddColumDocumentType : Migration
    {
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentType",
                table: "Persons");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DocumentType",
                table: "Persons",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}