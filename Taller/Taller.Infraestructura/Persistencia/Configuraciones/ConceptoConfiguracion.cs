using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;
using Taller.Dominio.Enumeraciones;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace Taller.Infraestructura.Persistencia.Configuraciones;

public class ConceptoConfiguracion
    : IEntityTypeConfiguration<Concepto>
{
    public void Configure(EntityTypeBuilder<Concepto> builder)
    {
        builder.ToTable("Conceptos");

        builder.HasKey(x => x.IdConcepto);

        builder.Property(x => x.Nombre)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.Descripcion)
            .HasMaxLength(500);

        builder.Property(x => x.Tipo)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.PrecioReferencia)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Activo)
            .HasDefaultValue(true);
        
        builder.HasOne(x => x.Categoria)
            .WithMany(x => x.Conceptos)
            .HasForeignKey(x => x.IdCategoria)
            .OnDelete(DeleteBehavior.NoAction);        

        // Datos iniciales necesarios para el funcionamiento del sistema.
        builder.HasData(
            new Concepto
            {
                IdConcepto = 1, 
                Nombre = "Servicio", 
                Descripcion = "Mano de obra, mantenimiento, diagnóstico y reparación de vehículos.", 
                Tipo = Taller.Dominio.Enumeraciones.TipoConcepto.Servicio, 
                PrecioReferencia = 15000.00m, 
                Activo = true 
            }, 
            new Concepto 
            { IdConcepto = 2, Nombre = "Consumible", 
                Descripcion = "Insumos y materiales consumibles utilizados en el taller.", 
                Tipo = Taller.Dominio.Enumeraciones.TipoConcepto.Consumible, 
                PrecioReferencia = 5000.00m, Activo = true 
            }, 
            new Concepto 
            { 
                IdConcepto = 3, 
                Nombre = "Repuestos", 
                Descripcion = "Piezas y componentes de reemplazo utilizados en la reparación de vehículos.", 
                Tipo = Taller.Dominio.Enumeraciones.TipoConcepto.Repuestos, 
                PrecioReferencia = 22000.00m, 
                Activo = true 
            } 
        );
    }
 }