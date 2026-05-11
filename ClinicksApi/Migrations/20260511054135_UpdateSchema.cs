using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicksApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "id_usuario",
                table: "medico",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_medico_id_usuario",
                table: "medico",
                column: "id_usuario");

            migrationBuilder.AddForeignKey(
                name: "fk_medico_usuario",
                table: "medico",
                column: "id_usuario",
                principalTable: "usuario",
                principalColumn: "id_usuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_medico_usuario",
                table: "medico");

            migrationBuilder.DropIndex(
                name: "IX_medico_id_usuario",
                table: "medico");

            migrationBuilder.DropColumn(
                name: "id_usuario",
                table: "medico");
        }
    }
}
