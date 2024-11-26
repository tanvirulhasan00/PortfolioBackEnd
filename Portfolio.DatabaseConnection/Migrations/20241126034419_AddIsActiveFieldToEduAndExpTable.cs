using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio.DatabaseConnection.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveFieldToEduAndExpTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Experiences",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Educations",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Educations");
        }
    }
}
