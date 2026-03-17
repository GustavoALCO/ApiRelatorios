using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace APIRelatorios.Infra.Migrations
{
    /// <inheritdoc />
    public partial class V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fiscais",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Login = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    HashPassword = table.Column<string>(type: "text", nullable: false),
                    Salt = table.Column<byte[]>(type: "bytea", nullable: false),
                    IsAdmin = table.Column<bool>(type: "boolean", nullable: false),
                    IsValid = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fiscais", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Rota",
                columns: table => new
                {
                    RotaId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NomeRota = table.Column<string>(type: "text", nullable: true),
                    Alimentador = table.Column<string>(type: "text", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataFinal = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rota", x => x.RotaId);
                });

            migrationBuilder.CreateTable(
                name: "EvidenciaRota",
                columns: table => new
                {
                    EvidenciaRotaId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FiscalId = table.Column<int>(type: "integer", nullable: false),
                    RotaId = table.Column<int>(type: "integer", nullable: false),
                    TemaFiscalizacao = table.Column<int>(type: "integer", nullable: false),
                    Alimentador = table.Column<string>(type: "text", nullable: true),
                    Identificacão = table.Column<string>(type: "text", nullable: true),
                    Descricao = table.Column<string>(type: "text", nullable: true),
                    ImageURL = table.Column<List<string>>(type: "text[]", nullable: false),
                    Endereco = table.Column<string>(type: "text", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    Horario = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvidenciaRota", x => x.EvidenciaRotaId);
                    table.ForeignKey(
                        name: "FK_EvidenciaRota_Rota_RotaId",
                        column: x => x.RotaId,
                        principalTable: "Rota",
                        principalColumn: "RotaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioRotas",
                columns: table => new
                {
                    RotaId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioRotas", x => new { x.UserId, x.RotaId });
                    table.ForeignKey(
                        name: "FK_UsuarioRotas_Fiscais_UserId",
                        column: x => x.UserId,
                        principalTable: "Fiscais",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioRotas_Rota_RotaId",
                        column: x => x.RotaId,
                        principalTable: "Rota",
                        principalColumn: "RotaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EvidenciaRota_RotaId",
                table: "EvidenciaRota",
                column: "RotaId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioRotas_RotaId",
                table: "UsuarioRotas",
                column: "RotaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EvidenciaRota");

            migrationBuilder.DropTable(
                name: "UsuarioRotas");

            migrationBuilder.DropTable(
                name: "Fiscais");

            migrationBuilder.DropTable(
                name: "Rota");
        }
    }
}
