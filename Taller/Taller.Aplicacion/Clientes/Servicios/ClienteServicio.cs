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
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        // Normalizar y validar campos obligatorios
        var nombre = dto.Nombre?.Trim();
        var apellido = dto.Apellido?.Trim();
        var docTipo = dto.DocTipo?.Trim();
        var docNro = dto.DocNro?.Trim();

        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre es obligatorio.", nameof(dto));

        if (string.IsNullOrWhiteSpace(apellido))
            throw new ArgumentException("El apellido es obligatorio.", nameof(dto));

        if (string.IsNullOrWhiteSpace(docTipo))
            throw new ArgumentException("El tipo de documento es obligatorio.", nameof(dto));

        if (string.IsNullOrWhiteSpace(docNro))
            throw new ArgumentException("El número de documento es obligatorio.", nameof(dto));

        // Verificar que no exista ya un cliente con ese documento
        var existeDoc = await _repositorio.ExistePorDocumentoAsync(docNro!);
        if (existeDoc)
            throw new InvalidOperationException("Ya existe un cliente con ese documento.");

        // Normalizar opcionales: si vienen vacíos, guardamos null
        string? telefono = string.IsNullOrWhiteSpace(dto.Telefono) ? null : dto.Telefono.Trim();
        string? email = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email.Trim();
        string? direccion = string.IsNullOrWhiteSpace(dto.Direccion) ? null : dto.Direccion.Trim();

        var cliente = new Cliente
        {
            Nombre = nombre!,     // ya validado
            Apellido = apellido!,   // ya validado
            DocTipo = docTipo!,    // ya validado
            DocNro = docNro!,     // ya validado
            Telefono = telefono,
            Email = email,
            Direccion = direccion,
            Activo = true
        };

        await _repositorio.AgregarAsync(cliente);
        return cliente.Id;
    }

    public async Task ActualizarAsync(ClienteDto dto)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        if (dto.Id <= 0)
            throw new ArgumentException("Id de cliente inválido.", nameof(dto));

        var cliente = await _repositorio.ObtenerPorIdAsync(dto.Id)
                      ?? throw new InvalidOperationException("Cliente no encontrado.");

        // Normalizar y validar obligatorios
        var nombre = dto.Nombre?.Trim();
        var apellido = dto.Apellido?.Trim();
        var docTipo = dto.DocTipo?.Trim();
        var docNro = dto.DocNro?.Trim();

        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre es obligatorio.", nameof(dto));

        if (string.IsNullOrWhiteSpace(apellido))
            throw new ArgumentException("El apellido es obligatorio.", nameof(dto));

        if (string.IsNullOrWhiteSpace(docTipo))
            throw new ArgumentException("El tipo de documento es obligatorio.", nameof(dto));

        if (string.IsNullOrWhiteSpace(docNro))
            throw new ArgumentException("El número de documento es obligatorio.", nameof(dto));

        // Validar unicidad de documento, excluyendo al propio cliente
        var existeDoc = await _repositorio.ExistePorDocumentoAsync(docNro!, dto.Id);
        if (existeDoc)
            throw new InvalidOperationException("Ya existe otro cliente con ese documento.");

        // Normalizar opcionales
        string? telefono = string.IsNullOrWhiteSpace(dto.Telefono) ? null : dto.Telefono.Trim();
        string? email = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email.Trim();
        string? direccion = string.IsNullOrWhiteSpace(dto.Direccion) ? null : dto.Direccion.Trim();

        // Mapear cambios a la entidad
        cliente.Nombre = nombre!;
        cliente.Apellido = apellido!;
        cliente.DocTipo = docTipo!;
        cliente.DocNro = docNro!;
        cliente.Telefono = telefono;
        cliente.Email = email;
        cliente.Direccion = direccion;
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
