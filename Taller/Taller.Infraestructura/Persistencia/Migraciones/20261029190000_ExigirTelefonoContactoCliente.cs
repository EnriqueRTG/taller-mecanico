using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taller.Infraestructura.Persistencia.Migraciones;

/// <summary>Exige un teléfono de contacto válido sin sustituir datos existentes por números ficticios.</summary>
public partial class ExigirTelefonoContactoCliente : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            IF EXISTS (SELECT 1 FROM dbo.Clientes WHERE Telefono IS NULL OR NOT ([Telefono] COLLATE Latin1_General_100_BIN2 NOT LIKE N'%[^0-9 +()-]%' AND LEN(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE([Telefono], N' ', N''), N'+', N''), N'(', N''), N')', N''), N'-', N'')) BETWEEN 7 AND 15 AND (CHARINDEX(N'+', [Telefono]) = 0 OR (LEFT(LTRIM([Telefono]), 1) = N'+' AND CHARINDEX(N'+', LTRIM([Telefono]), 2) = 0))))
                THROW 51142, N'Hay clientes sin teléfono de contacto válido. Complete los teléfonos reales (7 a 15 dígitos) antes de actualizar. No se modificaron los registros.', 1;
            """);
        // El SQL explícito evita que AlterColumn sustituya valores nulos por cadenas vacías.
        migrationBuilder.Sql("ALTER TABLE dbo.Clientes ALTER COLUMN Telefono nvarchar(30) NOT NULL;");
        migrationBuilder.AddCheckConstraint("CK_Clientes_TelefonoContacto", "Clientes", "[Telefono] COLLATE Latin1_General_100_BIN2 NOT LIKE N'%[^0-9 +()-]%' AND LEN(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE([Telefono], N' ', N''), N'+', N''), N'(', N''), N')', N''), N'-', N'')) BETWEEN 7 AND 15 AND (CHARINDEX(N'+', [Telefono]) = 0 OR (LEFT(LTRIM([Telefono]), 1) = N'+' AND CHARINDEX(N'+', LTRIM([Telefono]), 2) = 0))");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropCheckConstraint("CK_Clientes_TelefonoContacto", "Clientes");
        migrationBuilder.AlterColumn<string>(name: "Telefono", table: "Clientes", type: "nvarchar(30)", maxLength: 30,
            nullable: true, oldClrType: typeof(string), oldType: "nvarchar(30)", oldMaxLength: 30);
    }
}
