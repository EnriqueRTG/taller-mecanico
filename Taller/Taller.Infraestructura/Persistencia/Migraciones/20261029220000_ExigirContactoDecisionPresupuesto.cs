using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable
namespace Taller.Infraestructura.Persistencia.Migraciones;

/// <summary>Alinea las decisiones persistidas con los datos obligatorios de contacto y canal.</summary>
public partial class ExigirContactoDecisionPresupuesto : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // No inventar decisiones ni borrar propuestas para permitir actualizar.
        migrationBuilder.Sql("""
            IF EXISTS (SELECT 1 FROM dbo.Presupuestos WHERE NOT (
                [Estado] NOT IN (N'Aceptado', N'Rechazado', N'PendienteRevisionTecnica') OR ([PersonaDecision] IS NOT NULL AND LEN(LTRIM(RTRIM([PersonaDecision]))) BETWEEN 1 AND 150 AND [MedioDecision] IS NOT NULL AND [MedioDecision] IN (N'Presencial', N'Teléfono', N'Correo', N'Mensajería'))))
                THROW 51144, 'Hay decisiones de presupuesto sin persona o medio válido. Revise los datos reales antes de actualizar.', 1;
            """);
        migrationBuilder.AddCheckConstraint(
            name: "CK_Presupuestos_ContactoDecision", table: "Presupuestos",
            sql: "[Estado] NOT IN (N'Aceptado', N'Rechazado', N'PendienteRevisionTecnica') OR ([PersonaDecision] IS NOT NULL AND LEN(LTRIM(RTRIM([PersonaDecision]))) BETWEEN 1 AND 150 AND [MedioDecision] IS NOT NULL AND [MedioDecision] IN (N'Presencial', N'Teléfono', N'Correo', N'Mensajería'))");
    }

    protected override void Down(MigrationBuilder migrationBuilder) =>
        migrationBuilder.DropCheckConstraint(name: "CK_Presupuestos_ContactoDecision", table: "Presupuestos");
}
