using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinimalApiCURD.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLeadModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FisrtName",
                table: "Leads",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "Comapany",
                table: "Leads",
                newName: "Company");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Leads",
                newName: "FisrtName");

            migrationBuilder.RenameColumn(
                name: "Company",
                table: "Leads",
                newName: "Comapany");
        }
    }
}
