using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcreditacionesApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class FechaCumplimientoNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "fecha_cumplimiento",
                table: "requisitos_acreditacion",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "fecha_cumplimiento",
                table: "requisitos_acreditacion",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);
        }
    }
}
