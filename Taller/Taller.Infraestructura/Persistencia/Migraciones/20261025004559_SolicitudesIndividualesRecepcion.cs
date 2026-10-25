using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taller.Infraestructura.Persistencia.Migraciones
{
    /// <inheritdoc />
    public partial class SolicitudesIndividualesRecepcion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MedioDecision",
                table: "Presupuestos",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonaDecision",
                table: "Presupuestos",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdSolicitudAtencion",
                table: "PresupuestoDetalles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KilometrajeRecepcion",
                table: "Atenciones",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObservacionesRecepcion",
                table: "Atenciones",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SolicitudesAtencion",
                columns: table => new
                {
                    IdSolicitudAtencion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAtencion = table.Column<int>(type: "int", nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudesAtencion", x => x.IdSolicitudAtencion);
                    table.CheckConstraint("CK_SolicitudesAtencion_Descripcion", "LEN(LTRIM(RTRIM([Descripcion]))) > 0");
                    table.CheckConstraint("CK_SolicitudesAtencion_Orden", "[Orden] > 0");
                    table.CheckConstraint("CK_SolicitudesAtencion_Tipo", "[Tipo] IN (N'TrabajoSolicitado', N'ProblemaInformado')");
                    table.ForeignKey(
                        name: "FK_SolicitudesAtencion_Atenciones_IdAtencion",
                        column: x => x.IdAtencion,
                        principalTable: "Atenciones",
                        principalColumn: "IdAtencion",
                        onDelete: ReferentialAction.Restrict);
                });

            // Conserva el texto original como un único pedido en las atenciones anteriores.
            // No se intenta inferir ni dividir automáticamente una solicitud mixta histórica.
            migrationBuilder.Sql("""
                INSERT INTO [SolicitudesAtencion] ([IdAtencion], [Orden], [Tipo], [Descripcion])
                SELECT [IdAtencion], 1,
                    CASE WHEN [TipoSolicitud] = N'ServicioDefinido' THEN N'TrabajoSolicitado' ELSE N'ProblemaInformado' END,
                    [MotivoConsulta]
                FROM [Atenciones]
                WHERE LEN(LTRIM(RTRIM([MotivoConsulta]))) > 0;
                """);

            migrationBuilder.CreateIndex(
                name: "IX_PresupuestoDetalles_IdSolicitudAtencion",
                table: "PresupuestoDetalles",
                column: "IdSolicitudAtencion");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Atenciones_KilometrajeRecepcion",
                table: "Atenciones",
                sql: "[KilometrajeRecepcion] IS NULL OR [KilometrajeRecepcion] >= 0");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesAtencion_IdAtencion_Orden",
                table: "SolicitudesAtencion",
                columns: new[] { "IdAtencion", "Orden" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PresupuestoDetalles_SolicitudesAtencion_IdSolicitudAtencion",
                table: "PresupuestoDetalles",
                column: "IdSolicitudAtencion",
                principalTable: "SolicitudesAtencion",
                principalColumn: "IdSolicitudAtencion",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PresupuestoDetalles_SolicitudesAtencion_IdSolicitudAtencion",
                table: "PresupuestoDetalles");

            migrationBuilder.DropTable(
                name: "SolicitudesAtencion");

            migrationBuilder.DropIndex(
                name: "IX_PresupuestoDetalles_IdSolicitudAtencion",
                table: "PresupuestoDetalles");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Atenciones_KilometrajeRecepcion",
                table: "Atenciones");

            migrationBuilder.DropColumn(
                name: "MedioDecision",
                table: "Presupuestos");

            migrationBuilder.DropColumn(
                name: "PersonaDecision",
                table: "Presupuestos");

            migrationBuilder.DropColumn(
                name: "IdSolicitudAtencion",
                table: "PresupuestoDetalles");

            migrationBuilder.DropColumn(
                name: "KilometrajeRecepcion",
                table: "Atenciones");

            migrationBuilder.DropColumn(
                name: "ObservacionesRecepcion",
                table: "Atenciones");
        }
    }
}
