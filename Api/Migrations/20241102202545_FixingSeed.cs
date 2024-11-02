using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dio_lab_trilha_net_minimal_api_desafio.Migrations
{
    /// <inheritdoc />
    public partial class FixingSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Administradores",
                keyColumn: "Id",
                keyValue: 1,
                column: "Perfil",
                value: "Adm");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Administradores",
                keyColumn: "Id",
                keyValue: 1,
                column: "Perfil",
                value: "Admn");
        }
    }
}
