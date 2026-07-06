using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIRelatorios.Infra.Migrations
{
    /// <inheritdoc />
    public partial class V4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Emergencial",
                table: "EvidenciaRota");

            migrationBuilder.AddColumn<int>(
                name: "NivelRisco",
                table: "EvidenciaRota",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NivelRisco",
                table: "EvidenciaRota");

            migrationBuilder.AddColumn<bool>(
                name: "Emergencial",
                table: "EvidenciaRota",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
