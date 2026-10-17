using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;
namespace Taller.Infraestructura.Persistencia.Configuraciones;

public class CategoriaConfiguracion
    : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("Categorias");

        builder.HasKey(x => x.IdCategoria);

        builder.Property(x => x.Nombre)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(x => x.Nombre)
            .IsUnique();

        builder.Property(x => x.Descripcion)
            .HasMaxLength(250);

        builder.Property(x => x.Activo)
            .HasDefaultValue(true);

        // Datos iniciales necesarios para el funcionamiento del sistema.
        builder.HasData(
            new Categoria
            {
                IdCategoria = 1,
                Nombre = "Mantenimiento preventivo",
                Descripcion = "Servicios de mantenimiento preventivo y cambio de fluidos.",
                Activo = true
            },
            new Categoria
            {
                IdCategoria = 2,
                Nombre = "Reparación mecánica",
                Descripcion = "Servicios de reparación y sustitución de componentes mecánicos.",
                Activo = true
            },
            new Categoria
            {
                IdCategoria = 3,
                Nombre = "Diagnóstico electrónico",
                Descripcion = "Servicios de diagnóstico y reparación de sistemas electrónicos.",
                Activo = true
            },
            new Categoria
            {
                IdCategoria = 4,
                Nombre = "Sistema de frenos",
                Descripcion = "Servicios de reparación y mantenimiento del sistema de frenos.",
                Activo = true
            },
            new Categoria
            {
                IdCategoria = 5,
                Nombre = "Suspensión",
                Descripcion = "Servicios de reparación y mantenimiento de la suspensión.",
                Activo = true
            },
            new Categoria
            {
                IdCategoria = 6,
                Nombre = "Dirección",
                Descripcion = "Servicios de reparación y mantenimiento de la dirección.",
                Activo = true
            },
            new Categoria
            {
                IdCategoria = 7,
                Nombre = "Transmisión",
                Descripcion = "Servicios de reparación y mantenimiento de la transmisión.",
                Activo = true
            },
            new Categoria
            {
                IdCategoria = 8,
                Nombre = "Embrague",
                Descripcion = "Servicios de reparación y mantenimiento del embrague.",
                Activo = true
            },
            new Categoria
            {
                IdCategoria = 9,
                Nombre = "Sistema de refrigeración",
                Descripcion = "Servicios de reparación y mantenimiento del sistema de refrigeración.",
                Activo = true
            },
            new Categoria
            {
                IdCategoria = 10,
                Nombre = "Diagnóstico computarizado",
                Descripcion = "Servicios de diagnóstico computarizado del vehículo.",
                Activo = true
            },
            new Categoria
            {
                IdCategoria = 11,
                Nombre = "Sistema de arranque y carga",
                Descripcion = "Servicios de reparación del sistema de arranque y carga.",
                Activo = true
            },
            new Categoria
            {
                IdCategoria = 12,
                Nombre = "Baterías",
                Descripcion = "Servicios de revisión y reparación de baterías.",
                Activo = true
            },
            new Categoria
            {
                IdCategoria = 13,
                Nombre = "Electricidad e iluminación",
                Descripcion = "Servicios de reparación de luces y componentes eléctricos.",
                Activo = true
            },
            new Categoria
            {
                IdCategoria = 14,
                Nombre = "Aire acondicionado",
                Descripcion = "Servicios de reparación del sistema de climatización.",
                Activo = true
            },
            new Categoria
            {
                IdCategoria = 15,
                Nombre = "Calefacción",
                Descripcion = "Servicios de revisión y reparación de la calefacción.",
                Activo = true
            },
            new Categoria
            {
                IdCategoria = 16,
                Nombre = "Diagnóstico técnico inicial",
                Descripcion = "Servicios de inspección y evaluación general del vehículo.",
                Activo = true
            }
        );
    }
}