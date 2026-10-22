using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taller.Dominio.Entidades;
namespace Taller.Infraestructura.Persistencia.Configuraciones;
/// <summary>Configura integridad, relaciones e importes del circuito económico.</summary>
public sealed class MedioPagoConfiguracion : IEntityTypeConfiguration<MedioPago>
{
    public void Configure(EntityTypeBuilder<MedioPago> b)
    {
        b.ToTable("MediosPago"); b.HasKey(x => x.IdMedioPago);
        b.Property(x => x.Nombre).HasMaxLength(60).IsRequired(); b.HasIndex(x => x.Nombre).IsUnique();
        b.HasData(new MedioPago { IdMedioPago = 1, Nombre = "Efectivo" },
            new MedioPago { IdMedioPago = 2, Nombre = "Transferencia", RequiereReferencia = true },
            new MedioPago { IdMedioPago = 3, Nombre = "Tarjeta de débito", RequiereReferencia = true },
            new MedioPago { IdMedioPago = 4, Nombre = "Tarjeta de crédito", RequiereReferencia = true },
            new MedioPago { IdMedioPago = 5, Nombre = "Billetera virtual", RequiereReferencia = true });
    }
}
