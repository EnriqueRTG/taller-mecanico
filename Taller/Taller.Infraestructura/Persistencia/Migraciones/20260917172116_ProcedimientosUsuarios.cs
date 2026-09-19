using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taller.Infraestructura.Persistencia.Migraciones
{
    /// <inheritdoc />
    public partial class ProcedimientosUsuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                CREATE PROCEDURE dbo.Usuario_Crear
                    @NombreUsuario nvarchar(50),
                    @PasswordHash nvarchar(255),
                    @Nombre nvarchar(100),
                    @Apellido nvarchar(100),
                    @RolId int,
                    @FechaAlta datetime2,
                    @Id int OUTPUT
                AS
                BEGIN

                    SET NOCOUNT ON;

                    BEGIN TRY

                        INSERT INTO dbo.Usuarios
                            (NombreUsuario, PasswordHash, Nombre, Apellido, RolId, FechaAlta, Activo)
                        VALUES
                            (@NombreUsuario, @PasswordHash, @Nombre, @Apellido, @RolId, @FechaAlta, 1);

                        SET @Id = CONVERT(int, SCOPE_IDENTITY());
                    END TRY

                    BEGIN CATCH

                        IF ERROR_NUMBER() IN (2601, 2627)
                        
                        BEGIN
                            THROW 51001, 'El nombre de usuario ya esta registrado.', 1;
                        END;

                        THROW;

                    END CATCH;

                END;
                """);

            migrationBuilder.Sql(
                """
                CREATE PROCEDURE dbo.Usuario_ActualizarDatos
                    @Id int,
                    @Nombre nvarchar(100),
                    @Apellido nvarchar(100),
                    @RolId int
                AS
                BEGIN
                    SET NOCOUNT ON;
                    SET XACT_ABORT ON;

                    BEGIN TRY
                        
                        BEGIN TRANSACTION;

                            DECLARE @RolActualId int;
                            DECLARE @Activo bit;

                            SELECT
                                @RolActualId = RolId,
                                @Activo = Activo
                            FROM dbo.Usuarios WITH (TABLOCKX, HOLDLOCK)
                            WHERE Id = @Id;              

                        IF @RolActualId IS NULL
                            THROW 51002, 'El usuario no existe.', 1;

                        IF @RolActualId = 1
                            AND @Activo = 1
                            AND @RolId <> 1
                            AND NOT EXISTS
                            (
                                SELECT 1
                                FROM dbo.Usuarios
                                WHERE RolId = 1
                                    AND Activo = 1
                                    AND Id <> @Id
                            )
                            THROW 51003, 'No se puede cambiar el rol del ultimo administrador activo.', 1;

                        UPDATE dbo.Usuarios
                        SET Nombre = @Nombre,
                            Apellido = @Apellido,
                            RolId = @RolId
                        WHERE Id = @Id;

                        COMMIT TRANSACTION;

                    END TRY

                    BEGIN CATCH
                        
                        IF @@TRANCOUNT > 0
                            ROLLBACK TRANSACTION;

                        THROW;
                    
                    END CATCH

                END;
                """);

            migrationBuilder.Sql(
                """
                CREATE PROCEDURE dbo.Usuario_ActualizarCredenciales
                    @Id int,
                    @NombreUsuario nvarchar(50),
                    @PasswordHash nvarchar(255) = NULL
                AS
                BEGIN
                    SET NOCOUNT ON;

                    BEGIN TRY

                        UPDATE dbo.Usuarios
                        SET NombreUsuario = @NombreUsuario,
                            PasswordHash = COALESCE(@PasswordHash, PasswordHash)
                        WHERE Id = @Id;

                        IF @@ROWCOUNT = 0
                            THROW 51002, 'El usuario no existe.', 1;
                    
                    END TRY

                    BEGIN CATCH
                        
                        IF ERROR_NUMBER() IN (2601, 2627)
                            THROW 51001, 'El nombre de usuario ya esta registrado.', 1;

                        THROW;

                    END CATCH;

                END;
                """);

            migrationBuilder.Sql(
                """
                CREATE PROCEDURE dbo.Usuario_CambiarEstado
                    @Id int,
                    @Activo bit
                AS
                BEGIN
                    SET NOCOUNT ON;
                    SET XACT_ABORT ON;

                    BEGIN TRY

                        BEGIN TRANSACTION;

                        DECLARE @RolActualId int;
                        DECLARE @ActivoActual bit;

                        SELECT
                            @RolActualId = RolId,
                            @ActivoActual = Activo
                        FROM dbo.Usuarios WITH (TABLOCKX, HOLDLOCK)
                        WHERE Id = @Id;

                        IF @RolActualId IS NULL
                            THROW 51002, 'El usuario no existe.', 1;

                        IF @Activo = 0
                           AND @ActivoActual = 1
                           AND @RolActualId = 1
                           AND NOT EXISTS
                           (
                               SELECT 1
                               FROM dbo.Usuarios
                               WHERE RolId = 1
                                 AND Activo = 1
                                 AND Id <> @Id
                           )
                            THROW 51004, 'No se puede desactivar al ultimo administrador activo.', 1;

                        UPDATE dbo.Usuarios
                        SET Activo = @Activo
                        WHERE Id = @Id;

                        COMMIT TRANSACTION;

                    END TRY

                    BEGIN CATCH

                        IF @@TRANCOUNT > 0
                            ROLLBACK TRANSACTION;

                        THROW;

                    END CATCH;

                END
                """);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                DROP PROCEDURE dbo.Usuario_CambiarEstado;
                """
             );

            migrationBuilder.Sql(
                """
                DROP PROCEDURE dbo.Usuario_ActualizarCredenciales;
                """
             );

            migrationBuilder.Sql(
                """
                DROP PROCEDURE dbo.Usuario_ActualizarDatos;
                """
            );

            migrationBuilder.Sql(
                """
                DROP PROCEDURE dbo.Usuario_Crear;           
                """
            );

           
        }
    }
}
