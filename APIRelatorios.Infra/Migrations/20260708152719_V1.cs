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
                    RotaId = table.Column<Guid>(type: "uuid", nullable: false),
                    NomeRota = table.Column<string>(type: "text", nullable: true),
                    Alimentador = table.Column<string>(type: "text", nullable: false),
                    Concessionarias = table.Column<int>(type: "integer", nullable: false),
                    Km = table.Column<double>(type: "double precision", nullable: true),
                    DataInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataFinal = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    isValid = table.Column<bool>(type: "boolean", nullable: false),
                    TipoFiscalizacao = table.Column<int>(type: "integer", nullable: false),
                    TipoRota = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rota", x => x.RotaId);
                });

            migrationBuilder.CreateTable(
                name: "Amostras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RotaId = table.Column<Guid>(type: "uuid", nullable: false),
                    SeqISA = table.Column<string>(type: "text", nullable: false),
                    SeqBaseFisica = table.Column<string>(type: "text", nullable: false),
                    VlrBase = table.Column<string>(type: "text", nullable: true),
                    DescricaoTUC = table.Column<string>(type: "text", nullable: false),
                    DescricaoTec = table.Column<string>(type: "text", nullable: false),
                    ODIEngenharia = table.Column<string>(type: "text", nullable: false),
                    Instalacao = table.Column<string>(type: "text", nullable: false),
                    Endereco = table.Column<string>(type: "text", nullable: true),
                    Municipio = table.Column<string>(type: "text", nullable: true),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true),
                    LatitudeUser = table.Column<double>(type: "double precision", nullable: true),
                    LongitudeUser = table.Column<double>(type: "double precision", nullable: true),
                    TUC1 = table.Column<string>(type: "text", nullable: false),
                    TUC2 = table.Column<string>(type: "text", nullable: true),
                    TUC3 = table.Column<string>(type: "text", nullable: true),
                    TUC4 = table.Column<string>(type: "text", nullable: true),
                    TUC5 = table.Column<string>(type: "text", nullable: true),
                    TUC6 = table.Column<string>(type: "text", nullable: true),
                    NumSerie = table.Column<string>(type: "text", nullable: true),
                    PosicaoOperativa = table.Column<string>(type: "text", nullable: true),
                    Equipamento = table.Column<string>(type: "text", nullable: true),
                    DataFabricacao = table.Column<string>(type: "text", nullable: false),
                    Observacao = table.Column<string>(type: "text", nullable: false),
                    Fotos = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amostras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Amostras_Rota_RotaId",
                        column: x => x.RotaId,
                        principalTable: "Rota",
                        principalColumn: "RotaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EvidenciaRota",
                columns: table => new
                {
                    EvidenciaRotaId = table.Column<Guid>(type: "uuid", nullable: false),
                    FiscalId = table.Column<int>(type: "integer", nullable: false),
                    RotaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Alimentador = table.Column<string>(type: "text", nullable: true),
                    Identificacão = table.Column<string>(type: "text", nullable: true),
                    Descricao = table.Column<string>(type: "text", nullable: true),
                    Endereco = table.Column<string>(type: "text", nullable: false),
                    Cidade = table.Column<string>(type: "text", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    Horario = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NivelRisco = table.Column<int>(type: "integer", nullable: false),
                    IsValid = table.Column<bool>(type: "boolean", nullable: false)
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
                    RotaId = table.Column<Guid>(type: "uuid", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "CheckList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EvidenciaRotaId = table.Column<Guid>(type: "uuid", nullable: false),
                    TemaCheck = table.Column<int>(type: "integer", nullable: false),
                    SubTemaAlimentadores = table.Column<int[]>(type: "integer[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckList_EvidenciaRota_EvidenciaRotaId",
                        column: x => x.EvidenciaRotaId,
                        principalTable: "EvidenciaRota",
                        principalColumn: "EvidenciaRotaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OriginalUrl = table.Column<string>(type: "text", nullable: false),
                    LowUrl = table.Column<string>(type: "text", nullable: false),
                    EvidenciaRotaId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_EvidenciaRota_EvidenciaRotaId",
                        column: x => x.EvidenciaRotaId,
                        principalTable: "EvidenciaRota",
                        principalColumn: "EvidenciaRotaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Amostras_RotaId",
                table: "Amostras",
                column: "RotaId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckList_EvidenciaRotaId",
                table: "CheckList",
                column: "EvidenciaRotaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EvidenciaRota_RotaId",
                table: "EvidenciaRota",
                column: "RotaId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_EvidenciaRotaId",
                table: "Images",
                column: "EvidenciaRotaId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioRotas_RotaId",
                table: "UsuarioRotas",
                column: "RotaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Amostras");

            migrationBuilder.DropTable(
                name: "CheckList");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "UsuarioRotas");

            migrationBuilder.DropTable(
                name: "EvidenciaRota");

            migrationBuilder.DropTable(
                name: "Fiscais");

            migrationBuilder.DropTable(
                name: "Rota");
        }
    }
}
