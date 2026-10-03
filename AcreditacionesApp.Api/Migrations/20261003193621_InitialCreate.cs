using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AcreditacionesApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tipo_acreditacion",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    vigencia_meses = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tipo_acreditacion", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "acreditacion",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tipo_acreditacion_id = table.Column<int>(type: "integer", nullable: false),
                    nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    entidad_acreditadora = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    fecha_emision = table.Column<DateOnly>(type: "date", nullable: false),
                    fecha_vencimiento = table.Column<DateOnly>(type: "date", nullable: false),
                    estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_acreditacion", x => x.id);
                    table.ForeignKey(
                        name: "fk_acreditacion_tipo_acreditacion_tipo_acreditacion_id",
                        column: x => x.tipo_acreditacion_id,
                        principalTable: "tipo_acreditacion",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "requisitos_acreditacion",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    acreditacion_id = table.Column<int>(type: "integer", nullable: false),
                    descripcion = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    obligatorio = table.Column<bool>(type: "boolean", nullable: false),
                    cumplido = table.Column<bool>(type: "boolean", nullable: false),
                    fecha_cumplimiento = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_requisitos_acreditacion", x => x.id);
                    table.ForeignKey(
                        name: "fk_requisitos_acreditacion_acreditacion_acreditacion_id",
                        column: x => x.acreditacion_id,
                        principalTable: "acreditacion",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_acreditacion_tipo_acreditacion_id",
                table: "acreditacion",
                column: "tipo_acreditacion_id");

            migrationBuilder.CreateIndex(
                name: "ix_requisitos_acreditacion_acreditacion_id",
                table: "requisitos_acreditacion",
                column: "acreditacion_id");

            migrationBuilder.CreateIndex(
                name: "ix_tipo_acreditacion_nombre",
                table: "tipo_acreditacion",
                column: "nombre",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "requisitos_acreditacion");

            migrationBuilder.DropTable(
                name: "acreditacion");

            migrationBuilder.DropTable(
                name: "tipo_acreditacion");
        }
    }
}
