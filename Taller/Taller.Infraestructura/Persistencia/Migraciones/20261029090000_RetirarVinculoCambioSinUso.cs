using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taller.Infraestructura.Persistencia.Migraciones;

/// <summary>Retira un vínculo sin uso en el circuito de revisiones vigente.</summary>
public partial class RetirarVinculoCambioSinUso : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Nunca descartar silenciosamente un vínculo que una instalación haya utilizado.
        migrationBuilder.Sql("""
            IF EXISTS (SELECT 1 FROM dbo.PresupuestoDetalles WHERE IdPresupuestoCambio IS NOT NULL)
                THROW 51141, N'Hay vínculos de presupuesto de cambio ajenos al circuito actual. Revise esos datos antes de aplicar la limpieza; no se modificaron.', 1;
            """);
        migrationBuilder.DropForeignKey("FK_PresupuestoDetalles_Presupuestos_IdPresupuestoCambio", "PresupuestoDetalles");
        migrationBuilder.DropCheckConstraint("CK_PresupuestoDetalles_PresupuestoCambio", "PresupuestoDetalles");
        migrationBuilder.DropIndex("IX_PresupuestoDetalles_IdPresupuestoCambio", "PresupuestoDetalles");
        migrationBuilder.DropColumn("IdPresupuestoCambio", "PresupuestoDetalles");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>("IdPresupuestoCambio", "PresupuestoDetalles", type: "int", nullable: true);
        migrationBuilder.CreateIndex("IX_PresupuestoDetalles_IdPresupuestoCambio", "PresupuestoDetalles", "IdPresupuestoCambio");
        migrationBuilder.AddCheckConstraint("CK_PresupuestoDetalles_PresupuestoCambio", "PresupuestoDetalles", "[IdPresupuestoCambio] IS NULL OR [IdPresupuestoCambio] <> [IdPresupuesto]");
        migrationBuilder.AddForeignKey("FK_PresupuestoDetalles_Presupuestos_IdPresupuestoCambio", "PresupuestoDetalles", "IdPresupuestoCambio", "Presupuestos", principalColumn: "IdPresupuesto", onDelete: ReferentialAction.Restrict);
    }
}
