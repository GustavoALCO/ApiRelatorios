using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIRelatorios.Infra.Migrations
{
    /// <inheritdoc />
    public partial class V3corigido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Sincronizado",
                table: "Amostras",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sincronizado",
                table: "Amostras");
        }
    }
}
