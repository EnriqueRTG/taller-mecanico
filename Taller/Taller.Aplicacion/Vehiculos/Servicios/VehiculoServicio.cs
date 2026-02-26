using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taller.Aplicacion.Vehiculos.Dtos;
using Taller.Dominio.Repositorios;
using Taller.Dominio.Entidades;

namespace Taller.Aplicacion.Vehiculos.Servicios
{
    public sealed class VehiculoServicio : IVehiculoServicio
    {
        private readonly IVehiculoRepositorio _vehiculos;
        private readonly IClienteRepositorio _clientes;

        public VehiculoServicio(IVehiculoRepositorio vehiculos, IClienteRepositorio clientes)
        {
            _vehiculos = vehiculos;
            _clientes = clientes;
        }

        public async Task ActualizarAsync(VehiculoEdicionDto dto)
        {
            if (dto is null) 
                throw new ArgumentNullException("Id de vehiculo requerido para actualizar.", nameof(dto));

            var vehiculo = await _vehiculos.ObtenerPorIdAsync(dto.Id)
                ?? throw new InvalidOperationException($"No se encontró un vehículo con ID {dto.Id}.");

            var patente = dto.Patente?.Trim().ToUpper();
            var marca = dto.Marca?.Trim().ToUpper();

            if (string.IsNullOrWhiteSpace(patente))
                throw new ArgumentException("La patente es obligatoria.", nameof(dto));

            if (string.IsNullOrWhiteSpace(marca))
                throw new ArgumentException("La marca es obligatoria.", nameof(marca));

            if (await _vehiculos.ExistePorPatenteAsync(patente, dto.Id))
                throw new InvalidOperationException($"Ya existe un vehículo con la patente '{patente}'.");

            vehiculo.ClienteId = dto.ClienteId;
            vehiculo.Patente = patente;
            vehiculo.Marca = marca;
            vehiculo.Modelo = string.IsNullOrWhiteSpace(dto.Modelo) ? null : dto.Modelo.Trim();
            vehiculo.Anio = dto.Anio;
            vehiculo.Vin = string.IsNullOrWhiteSpace(dto.Vin) ? null : dto.Vin.Trim();
            vehiculo.Color = string.IsNullOrWhiteSpace(dto.Color) ? null : dto.Color.Trim();
            vehiculo.Observaciones = string.IsNullOrWhiteSpace(dto.Observaciones) ? null : dto.Observaciones.Trim();
            vehiculo.Activo = dto.Activo;

            await _vehiculos.ActualizarAsync(vehiculo);
        }

        public async Task<long> CrearAsync(VehiculoEdicionDto dto)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));

            var patente = dto.Patente?.Trim().ToUpper();
            var marca = dto.Marca?.Trim().ToUpper();

            if (string.IsNullOrWhiteSpace(patente)) 
                throw new ArgumentException("La patente es obligatoria.", nameof(dto));

            if (string.IsNullOrWhiteSpace(marca))
                throw new ArgumentException("La marca es obligatoria.", nameof(dto));

            var cliente = await _clientes.ObtenerPorIdAsync(dto.ClienteId)
                ?? throw new ArgumentException($"No se encontró un cliente con ID {dto.ClienteId}.", nameof(dto));

            var vehiculo = new Vehiculo
            {
                ClienteId = dto.ClienteId,
                Patente = patente,
                Marca = marca,
                Modelo = string.IsNullOrWhiteSpace(dto.Modelo) ? null : dto.Modelo.Trim(),
                Anio = dto.Anio,
                Vin = string.IsNullOrWhiteSpace(dto.Vin) ? null : dto.Vin.Trim(),
                Color = string.IsNullOrWhiteSpace(dto.Color) ? null : dto.Color.Trim(),
                Observaciones = string.IsNullOrWhiteSpace(dto.Color) ? null : dto.Color.Trim(),
                Activo = true
            };

            await _vehiculos.AgregarAsync(vehiculo);
            return vehiculo.Id;
        }

        public async Task EliminarAsync(long id)
        {
            var vehiculo = await _vehiculos.ObtenerPorIdAsync(id)
                ?? throw new InvalidOperationException($"No se encontró un vehículo con ID {id}.");

            await _vehiculos.EliminarAsync(vehiculo);
        }

        public async Task<IReadOnlyList<VehiculoDto>> ListarAsync()
        {
            var lista = await _vehiculos.ListarAsync();

            return lista.Select(v => new VehiculoDto
            {
                Id = v.Id,
                ClienteId = v.ClienteId,
                ClienteNombreCompleto = $"{v.Cliente.Apellido} {v.Cliente.Nombre}",
                Patente = v.Patente,
                Marca = v.Marca,
                Modelo = v.Modelo,
                Anio = v.Anio,
                Color = v.Color,
                Activo = (bool)v.Activo
            }).ToList();
        }

        public async Task<IReadOnlyList<VehiculoDto>> ListarPorClienteIdAsync(long clienteId)
        {
            var lista = await _vehiculos.ListarPorClienteAsync(clienteId);

            return lista.Select(v => new VehiculoDto
            {
                Id = v.Id,
                ClienteId = v.ClienteId,
                ClienteNombreCompleto = $"{v.Cliente.Apellido} {v.Cliente.Nombre}",
                Patente = v.Patente,
                Marca = v.Marca,
                Modelo = v.Modelo,
                Anio = v.Anio,
                Color = v.Color,
                Activo = (bool)v.Activo
            }).ToList();
        }

        public async Task<VehiculoEdicionDto?> ObtenerParaEdiciondAsync(long id)
        {
            var v = await _vehiculos.ObtenerPorIdAsync(id);

            if (v == null) return null;

            return new VehiculoEdicionDto
            {
                Id = v.Id,
                ClienteId = v.ClienteId,
                Patente = v.Patente,
                Marca = v.Marca,
                Modelo = v.Modelo,
                Anio = v.Anio,
                Vin = v.Vin,
                Color = v.Color,
                Observaciones = v.Observaciones,
                Activo = (bool)v.Activo
            };
        }
    }
}
