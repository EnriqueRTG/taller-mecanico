using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taller.Aplicacion.Clientes.Dtos;
using Taller.Dominio.Entidades;
using Taller.Dominio.Repositorios;

namespace Taller.Aplicacion.Clientes.Servicios;

public class ClienteServicio : IClienteServicio
{
    private readonly IClienteRepositorio _repositorio;

    public ClienteServicio(IClienteRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IReadOnlyList<ClienteDto>> ListarAsync()
    {
        var clientes = await _repositorio.ListarAsync();

        return clientes
            .Select(c => new ClienteDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                DocTipo = c.DocTipo,
                DocNro = c.DocNro,
                Telefono = c.Telefono,
                Email = c.Email,
                Direccion = c.Direccion,
                Activo = c.Activo
            })
            .ToList();
    }

    public async Task<ClienteDto?> ObtenerPorIdAsync(long id)
    {
        var c = await _repositorio.ObtenerPorIdAsync(id);
        if (c is null) return null;

        return new ClienteDto
        {
            Id = c.Id,
            Nombre = c.Nombre,
            Apellido = c.Apellido,
            DocTipo = c.DocTipo,
            DocNro = c.DocNro,
            Telefono = c.Telefono,
            Email = c.Email,
            Direccion = c.Direccion,
            Activo = c.Activo
        };
    }

    public async Task<long> CrearAsync(ClienteDto dto)
    {
        var existeDoc = await _repositorio.ExistePorDocumentoAsync(dto.DocNro);
        if (existeDoc)
            throw new InvalidOperationException("Ya existe un cliente con ese documento.");

        var cliente = new Cliente
        {
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            DocTipo = dto.DocTipo,
            DocNro  = dto.DocNro,
            Telefono = dto.Telefono,
            Email = dto.Email,
            Direccion = dto.Direccion,
            Activo = true,

        };

        await _repositorio.AgregarAsync(cliente);
        return cliente.Id;
    }

    public async Task ActualizarAsync(ClienteDto dto)
    {
        var cliente = await _repositorio.ObtenerPorIdAsync(dto.Id)
            ?? throw new InvalidOperationException("Cliente no encontrado.");

        var existeDoc = await _repositorio.ExistePorDocumentoAsync(dto.DocNro, dto.Id);
        if (existeDoc)
            throw new InvalidOperationException("Ya existe otro cliente con ese documento.");

        cliente.Nombre = dto.Nombre;
        cliente.Apellido = dto.Apellido;
        cliente.DocTipo = dto.DocTipo;
        cliente.DocNro = dto.DocNro;
        cliente.Telefono = dto.Telefono;
        cliente.Email = dto.Email;
        cliente.Direccion = dto.Direccion;
        cliente.Activo = dto.Activo;

        await _repositorio.ActualizarAsync(cliente);
    }

    public async Task EliminarAsync(long id)
    {
        var cliente = await _repositorio.ObtenerPorIdAsync(id)
            ?? throw new InvalidOperationException("Cliente no encontrado.");

        await _repositorio.EliminarAsync(cliente);
    }
}
