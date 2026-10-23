using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Taller.Aplicacion.DTOs.Atenciones;
using Taller.Aplicacion.DTOs.Cobros;
using Taller.Aplicacion.DTOs.Ejecuciones;
using Taller.Aplicacion.DTOs.Presupuestos;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Entidades;
using Taller.Dominio.Enumeraciones;
using Taller.Infraestructura.Persistencia;

namespace Taller.Demostracion;
/// <summary>Fixtures académicos generados por servicios reales. No invoca respaldos ni simula una auditoría de acceso.</summary>
public static class CargadorDemostracion
{
    public static async Task CargarAsync(IServiceProvider sp, string clave)
    {
        var db = sp.GetRequiredService<TallerDbContext>();
        var sesion = sp.GetRequiredService<SesionUsuario>();
        async Task Entrar(int id)
        {
            db.ChangeTracker.Clear();
            sesion.Iniciar(await db.Usuarios.Include(u => u.Rol).AsNoTracking().SingleAsync(u => u.Id == id));
        }

        var admin = await db.Usuarios.SingleAsync();
        await Entrar(admin.Id);
        var usuarios = sp.GetRequiredService<UsuarioServicio>();
        await usuarios.CrearAsync("demo.admin", clave, "Laura", "Gómez", 1);
        var inactivo = await usuarios.CrearAsync("demo.inactivo", clave, "Mario", "Giménez", 3);
        await usuarios.CambiarEstadoAsync(inactivo.Id, false, admin.Id);
        var recepcion = await usuarios.CrearAsync("demo.recepcion", clave, "Ana", "Pérez", 2);
        var recepcionDos = await usuarios.CrearAsync("demo.recepciondos", clave, "Martín", "López", 2);
        var tecnicos = new List<int>();
        foreach (var(login, nombre, apellido)in new[]
        {
            ("demo.tecnico", "Carlos", "Díaz"),
            ("demo.tecnicodos", "Lucía", "Romero"),
            ("demo.tecnicotres", "Diego", "Silva")
        }

        )
            tecnicos.Add((await usuarios.CrearAsync(login, clave, nombre, apellido, 3)).Id);
        await Entrar(recepcion.Id);
        var clientes = sp.GetRequiredService<ClienteServicio>();
        var vehiculos = sp.GetRequiredService<VehiculoServicio>();
        string[] nombres = ["María", "José", "Elena", "Pablo", "Sofía", "Pedro", "Valeria", "Jorge"];
        string[] apellidos = ["Fernández", "Torres", "Pérez", "García", "Romero"];
        var idsClientes = new List<int>();
        for (int i = 0; i < 40; i++)
            idsClientes.Add((await clientes.RegistrarAsync("DNI", (40000000 + i).ToString(), nombres[i % 8], apellidos[i / 8], "379400" + (1000 + i), $"demo{i + 1}@example.invalid", "Calle de prueba " + (100 + i))).IdCliente);
        var modelos = await db.Modelos.AsNoTracking().OrderBy(x => x.IdModelo).ToListAsync();
        var idsVehiculos = new List<int>();
        for (int i = 0; i < 100; i++)
            idsVehiculos.Add((await vehiculos.RegistrarAsync($"DM{i:000}AA", 2010 + i % 16, new[] { "Blanco", "Gris", "Azul", "Rojo", "Negro" }[i % 5], modelos[i % modelos.Count].IdModelo)).IdVehiculo);
        // Precios exclusivamente académicos. No constituyen valores comerciales ni se aplican a una base real.
        foreach (var concepto in await db.Conceptos.ToListAsync())
            concepto.PrecioReferencia = concepto.Tipo == TipoConcepto.Servicio ? 15000 + concepto.IdConcepto * 350 : 5000 + concepto.IdConcepto * 180;
        await db.SaveChangesAsync();
        db.ChangeTracker.Clear();
        // Los maestros y autores deben existir antes que las atenciones históricas del lote.
        var altaHistorica = DateTime.Today.AddDays(-200);
        foreach(var u in await db.Usuarios.ToListAsync()) u.FechaAlta=altaHistorica;
        foreach(var c in await db.Clientes.ToListAsync()) c.FechaAlta=altaHistorica;
        foreach(var v in await db.Vehiculos.ToListAsync()) v.FechaAlta=altaHistorica;
        await db.SaveChangesAsync(); db.ChangeTracker.Clear();
        var catalogo = await db.Conceptos.AsNoTracking().Where(c => c.Activo).ToListAsync();
        var servicios = catalogo.Where(c => c.Tipo == TipoConcepto.Servicio && !c.DisponibleParaIngreso).ToList();
        var repuestos = catalogo.Where(c => c.Tipo == TipoConcepto.Repuesto).ToList();
        var ingreso = catalogo.First(c => c.Tipo == TipoConcepto.Servicio && c.DisponibleParaIngreso);
        var recepcionServicio = sp.GetRequiredService<AtencionServicio>();
        var evaluacion = sp.GetRequiredService<EvaluacionServicio>();
        var presupuestos = sp.GetRequiredService<PresupuestoServicio>();
        var ejecucion = sp.GetRequiredService<EjecucionServicio>();
        var seguimiento = sp.GetRequiredService<SeguimientoTrabajoServicio>();
        var cobros = sp.GetRequiredService<CobroServicio>();
        string[] motivos = ["Ruido al frenar y vibración en el pedal", "Solicita reemplazo de filtro y aceite", "Pérdida de refrigerante al estacionar", "Solicita revisión de luces y batería", "Dificultad para arrancar en frío", "Solicita revisión de suspensión"];
        async Task<byte[]> Version(int id)
        {
            db.ChangeTracker.Clear();
            return (await db.Atenciones.AsNoTracking().SingleAsync(a => a.IdAtencion == id)).Version;
        }

        async Task<PresupuestoGestionDto> Actual(int id) => await presupuestos.ObtenerAsync(id);
        for (int i = 0; i < 100; i++)
        {
            int etapa = i % 16;
            int tecnico = tecnicos[i % tecnicos.Count];
            await Entrar(i % 2 == 0 ? recepcion.Id : recepcionDos.Id);
            bool definido = i % 3 == 1;
            var atencion = await recepcionServicio.RegistrarAsync(idsClientes[i % 40], idsVehiculos[i], sesion.UsuarioActual!.Id, $"DEMO-{i + 1:000} · {motivos[i % 6]}", definido ? TipoSolicitudAtencion.ServicioDefinido : TipoSolicitudAtencion.Problema);
            int id = atencion.IdAtencion;
            if (etapa == 0)
            {
                await FecharAsync(db, id, i);
                continue;
            }

            if (etapa == 15)
            {
                await evaluacion.CancelarAperturaAsync(id, await Version(id), "El cliente desiste antes de cualquier intervención.");
                await FecharAsync(db, id, i);
                continue;
            }

            if (!definido)
                await recepcionServicio.ConfirmarIngresoAsync(new() { IdAtencion = id, VersionAtencion = await Version(id), ClienteAutoriza = true, Servicios = [new() { IdConcepto = ingreso.IdConcepto, PrecioUnitario = 12000 }] });
            else
                await evaluacion.EnviarAEvaluacionAsync(id, TipoSolicitudAtencion.ServicioDefinido, await Version(id));
            if (etapa == 1)
            {
                await FecharAsync(db, id, i);
                continue;
            }

            await Entrar(tecnico);
            await evaluacion.TomarAsync(id);
            var datos = await evaluacion.ObtenerAsync(id);
            datos.IncluyeDiagnostico = !definido;
            datos.FallaDetectada = "Inspección de demostración: desgaste detectado y revisión técnica registrada.";
            datos.TrabajoRecomendado = "Reemplazo del elemento afectado, ajuste y comprobación de funcionamiento.";
            datos.Kilometraje = 35000 + i * 1250;
            datos.Observaciones = "Datos ficticios para el coloquio. Alcance relevado por el técnico.";
            var servicio = servicios[i % servicios.Count];
            var repuesto = repuestos[i % repuestos.Count];
            datos.Items = [new()
            {
                IdConcepto = servicio.IdConcepto,
                Descripcion = servicio.Nombre,
                Cantidad = 1,
                PrecioUnitario = 20000 + i * 150
            }, new()
            {
                IdConcepto = repuesto.IdConcepto,
                Descripcion = repuesto.Nombre,
                Cantidad = 1,
                PrecioUnitario = etapa == 10 ? 0 : 10000 + i * 100,
                OrigenMaterial = etapa == 10 ? OrigenMaterial.Cliente : OrigenMaterial.Taller
            }

            ];
            foreach (var inicial in datos.ServiciosIniciales)
                inicial.Estado = EstadoEjecucionDetalle.Completado;
            await evaluacion.GuardarAsync(datos, etapa != 2);
            if (etapa == 2)
            {
                await FecharAsync(db, id, i);
                continue;
            }

            var propuesta = await db.Presupuestos.AsNoTracking().SingleAsync(p => p.IdAtencion == id && p.Tipo == TipoPresupuesto.Reparacion && p.Vigente);
            if (etapa == 3)
            {
                await FecharAsync(db, id, i);
                continue;
            }

            await Entrar(recepcion.Id);
            await evaluacion.PresentarAsync(propuesta.IdPresupuesto, propuesta.Version);
            if (etapa == 4)
            {
                await FecharAsync(db, id, i);
                continue;
            }

            var gestion = await Actual(propuesta.IdPresupuesto);
            await presupuestos.DecidirAsync(new() { IdPresupuesto = gestion.IdPresupuesto, Version = gestion.Version, ItemsAceptados = etapa == 5 ? [] : etapa == 6 ? [gestion.Items[0].IdDetalle] : gestion.Items.Select(x => x.IdDetalle).ToList(), Observaciones = etapa == 5 ? "Cliente rechaza la propuesta y solicita coordinar el retiro." : etapa == 6 ? "Cliente solicita solo una parte; requiere revisión técnica." : "Cliente acepta el alcance y sus importes." });
            if (etapa == 6 && i >= 32)
            {
                await Entrar(tecnico);
                var original = await presupuestos.ObtenerAsync(gestion.IdPresupuesto);
                int revisado = await seguimiento.RevisarSeleccionAsync(original.IdPresupuesto, original.Version, [new() { IdConcepto = original.Items[0].IdConcepto, Descripcion = original.Items[0].Descripcion, Cantidad = original.Items[0].Cantidad, PrecioUnitario = original.Items[0].PrecioUnitario }], "Se revisan las dependencias: el alcance seleccionado es viable por separado.");
                await Entrar(recepcion.Id);
                var revision = await Actual(revisado);
                await evaluacion.PresentarAsync(revisado, revision.Version);
                revision = await Actual(revisado);
                await presupuestos.DecidirAsync(new() { IdPresupuesto = revisado, Version = revision.Version, ItemsAceptados = revision.Items.Select(x => x.IdDetalle).ToList(), Observaciones = "Acepta la nueva versión revisada por el técnico." });
            }

            if (etapa is 5 or 6 or 7)
            {
                await FecharAsync(db, id, i);
                continue;
            }

            await Entrar(tecnico);
            var trabajo = await ejecucion.ObtenerAsync(id);
            if (trabajo.IdTecnico == null)
                await ejecucion.TomarAsync(id, trabajo.Version, "Alcance completo revisado y viable.");
            if (etapa == 8)
            {
                trabajo = await ejecucion.ObtenerAsync(id);
                trabajo.Items[0].Estado = EstadoEjecucionDetalle.Completado;
                trabajo.Items[0].CantidadEjecutada = trabajo.Items[0].CantidadAutorizada;
                await ejecucion.GuardarAsync(trabajo, false);
                await FecharAsync(db, id, i);
                continue;
            }

            if (etapa == 9)
            {
                await seguimiento.RegistrarHallazgoAsync(new() { IdAtencion = id, Version = await Version(id), Descripcion = "Se detecta una pieza adicional deteriorada durante el desmontaje.", Recomendacion = "Reemplazar antes de continuar con el conjunto.", ImpideContinuar = true });
                var seg = await seguimiento.ObtenerAsync(id);
                await seguimiento.CrearAdicionalAsync(new() { IdAtencion = id, Version = seg.Version, IdHallazgo = seg.Hallazgos[0].Id, Motivo = "Reparación adicional surgida durante la ejecución.", Items = [new() { IdConcepto = servicios[(i + 1) % servicios.Count].IdConcepto, Descripcion = "Trabajo adicional por hallazgo", Cantidad = 1, PrecioUnitario = 18000 }] });
                await FecharAsync(db, id, i);
                continue;
            }

            if (etapa == 10)
            {
                await seguimiento.PausarAsync(id, await Version(id), null, "Esperando el repuesto que aporta el cliente.");
                if (i >= 32)
                {
                    var estadoMaterial = await seguimiento.ObtenerAsync(id);
                    var materialCliente = estadoMaterial.Items.Single(x => x.Origen == OrigenMaterial.Cliente);
                    await seguimiento.RecibirMaterialAsync(id, estadoMaterial.Version, materialCliente.IdDetalle, materialCliente.Cantidad, true, "Pieza recibida del cliente y revisada por el técnico; conforme.");
                    estadoMaterial = await seguimiento.ObtenerAsync(id);
                    await seguimiento.ResolverPausaAsync(id, estadoMaterial.Version, estadoMaterial.Pausas.Single(x => x.FechaResolucion == null).Id, "Se recibió y verificó el material. Puede continuar la tarea autorizada.");
                }

                await FecharAsync(db, id, i);
                continue;
            }

            trabajo = await ejecucion.ObtenerAsync(id);
            foreach (var item in trabajo.Items)
            {
                item.Estado = EstadoEjecucionDetalle.Completado;
                item.CantidadEjecutada = item.CantidadAutorizada;
            }

            trabajo.Observaciones = "Técnico confirma trabajos realizados y comprobación final satisfactoria.";
            await ejecucion.GuardarAsync(trabajo, true);
            if (etapa == 11)
            {
                await FecharAsync(db, id, i);
                continue;
            }

            await Entrar(recepcion.Id);
            var cuenta = await cobros.ObtenerAsync(id);
            await cobros.LiquidarAsync(id, cuenta.VersionAtencion, "Liquidación del trabajo autorizado y efectivamente realizado.");
            var comprobante = (await cobros.ObtenerAsync(id)).Comprobante!;
            if (etapa >= 13)
            {
                decimal parcial = decimal.Round(comprobante.Total / 3, 2);
                var primerImporte = decimal.Round(parcial / 2, 2);
                int recibo = await cobros.RegistrarPagoAsync(new() { IdComprobante = comprobante.IdComprobante, Version = comprobante.Version, Medios = [new() { IdMedioPago = 1 + i % 5, Importe = primerImporte, Referencia = $"DEMO-A-{i}" }, new() { IdMedioPago = 1 + (i + 1) % 5, Importe = parcial - primerImporte, Referencia = $"DEMO-B-{i}" }] });
                if (etapa == 13 && i >= 32)
                {
                    comprobante = (await cobros.ObtenerAsync(id)).Comprobante!;
                    await cobros.AnularPagoAsync(comprobante.IdComprobante, recibo, comprobante.Version, "Se anula el cobro registrado por medio equivocado; se conserva la evidencia.");
                    comprobante = (await cobros.ObtenerAsync(id)).Comprobante!;
                    await cobros.RegistrarPagoAsync(new() { IdComprobante = comprobante.IdComprobante, Version = comprobante.Version, Medios = [new() { IdMedioPago = 2, Importe = parcial, Referencia = $"DEMO-CORREGIDO-{i}" }] });
                }

                comprobante = (await cobros.ObtenerAsync(id)).Comprobante!;
                if (etapa == 14)
                {
                    await cobros.RegistrarPagoAsync(new() { IdComprobante = comprobante.IdComprobante, Version = comprobante.Version, Medios = [new() { IdMedioPago = 2, Importe = comprobante.Saldo, Referencia = $"DEMO-TRX-{i + 1:000}" }] });
                    await cobros.EntregarAsync(id, (await cobros.ObtenerAsync(id)).VersionAtencion, "Entrega bajo conformidad del cliente; saldo cero y trabajos informados.");
                }
            }

            await FecharAsync(db, id, i);
        }

        await Entrar(recepcion.Id);
        foreach(var idVehiculo in await db.Atenciones.Where(a=>a.Estado==EstadoAtencion.Cerrada).OrderBy(a=>a.IdAtencion).Select(a=>a.IdVehiculo).Take(5).ToListAsync())
        {
            var v=await vehiculos.ObtenerPorIdAsync(idVehiculo);
            await vehiculos.DarDeBajaAsync(idVehiculo,v!.Version);
        }
        // Registros inactivos para filtros. Solo maestros sin atenciones asociadas.
        for (int i = 0; i < 5; i++)
        {
            var c = await clientes.RegistrarAsync("DNI", (49000000 + i).ToString(), "Cliente", "Inactivo", null, null, null);
            var actual = await clientes.ObtenerPorIdAsync(c.IdCliente);
            await clientes.DarDeBajaAsync(c.IdCliente, actual!.Version);
        }
    }

    private static async Task FecharAsync(TallerDbContext db, int id, int indice)
    {
        db.ChangeTracker.Clear();
        // Retrodatación exclusivamente de fixtures, conserva los intervalos y el orden interno de cada circuito.
        int dias = indice * 7 % 180;
        var a = await db.Atenciones.SingleAsync(a => a.IdAtencion == id);
        a.FechaApertura = a.FechaApertura.AddDays(-dias);
        a.FechaCierre = a.FechaCierre?.AddDays(-dias);
        foreach (var h in await db.HistorialAtenciones.Where(x => x.IdAtencion == id).ToListAsync())
            h.Fecha = h.Fecha.AddDays(-dias);
        foreach (var d in await db.Diagnosticos.Where(x => x.IdAtencion == id).ToListAsync())
            d.FechaDiagnostico = d.FechaDiagnostico.AddDays(-dias);
        foreach (var p in await db.Presupuestos.Where(x => x.IdAtencion == id).ToListAsync())
        {
            p.FechaCreacion = p.FechaCreacion.AddDays(-dias);
            p.FechaPresentacion = p.FechaPresentacion?.AddDays(-dias);
            p.FechaDecision = p.FechaDecision?.AddDays(-dias);
        }

        foreach (var d in await db.PresupuestoDetalles.Where(d => db.Presupuestos.Any(p => p.IdPresupuesto == d.IdPresupuesto && p.IdAtencion == id)).ToListAsync())
        {
            d.FechaRegistroEjecucion = d.FechaRegistroEjecucion?.AddDays(-dias);
            d.FechaRecepcionMaterial = d.FechaRecepcionMaterial?.AddDays(-dias);
        }

        foreach (var c in await db.Comprobantes.Where(x => x.IdAtencion == id).ToListAsync())
            c.FechaEmision = c.FechaEmision.AddDays(-dias);
        foreach (var p in await db.Pagos.Where(p => db.Comprobantes.Any(c => c.IdComprobante == p.IdComprobante && c.IdAtencion == id)).ToListAsync())
        {
            p.Fecha = p.Fecha.AddDays(-dias);
            p.FechaAnulacion = p.FechaAnulacion?.AddDays(-dias);
        }

        foreach (var h in await db.Set<HallazgoAtencion>().Where(x => x.IdAtencion == id).ToListAsync())
            h.Fecha = h.Fecha.AddDays(-dias);
        foreach (var p in await db.Set<PausaTrabajo>().Where(x => x.IdAtencion == id).ToListAsync())
        {
            p.Fecha = p.Fecha.AddDays(-dias);
            p.FechaResolucion = p.FechaResolucion?.AddDays(-dias);
        }

        foreach (var e in await db.Set<EvaluacionAtencion>().Where(x => x.IdAtencion == id).ToListAsync())
            e.FechaActualizacion = e.FechaActualizacion.AddDays(-dias);
        await db.SaveChangesAsync();
        db.ChangeTracker.Clear();
    }
}
