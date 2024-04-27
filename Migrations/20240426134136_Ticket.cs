using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourismMVC.Migrations
{
    /// <inheritdoc />
    public partial class Ticket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Ticket",
                table: "Attraction",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ticket",
                table: "Attraction");
        }
    }
}
