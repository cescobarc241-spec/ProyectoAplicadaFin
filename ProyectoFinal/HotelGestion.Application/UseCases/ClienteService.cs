using System;
using System.Collections.Generic;
using System.Text;

using HotelGestion.Application.DTOs;
using HotelGestion.Application.Interfaces;
using HotelGestion.Domain.Entities;
using HotelGestion.Domain.Interfaces;

namespace HotelGestion.Application.UseCases;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _clienteRepository;

    public ClienteService(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public List<ClienteDto> ObtenerTodos()
    {
        var clientes = _clienteRepository.ObtenerTodos();

        return clientes.Select(c => new ClienteDto
        {
            ClienteId = c.ClienteId,
            TipoDocumento = c.TipoDocumento,
            NumeroDocumento = c.NumeroDocumento,
            Nombres = c.Nombres,
            Apellidos = c.Apellidos,
            Telefono = c.Telefono,
            Correo = c.Correo,
            Direccion = c.Direccion
        }).ToList();
    }

    public ClienteDto? ObtenerPorId(int clienteId)
    {
        var cliente = _clienteRepository.ObtenerPorId(clienteId);

        if (cliente == null)
            return null;

        return new ClienteDto
        {
            ClienteId = cliente.ClienteId,
            TipoDocumento = cliente.TipoDocumento,
            NumeroDocumento = cliente.NumeroDocumento,
            Nombres = cliente.Nombres,
            Apellidos = cliente.Apellidos,
            Telefono = cliente.Telefono,
            Correo = cliente.Correo,
            Direccion = cliente.Direccion
        };
    }

    public int Registrar(ClienteDto cliente)
    {
        var entidad = new Cliente
        {
            TipoDocumento = cliente.TipoDocumento,
            NumeroDocumento = cliente.NumeroDocumento,
            Nombres = cliente.Nombres,
            Apellidos = cliente.Apellidos,
            Telefono = cliente.Telefono,
            Correo = cliente.Correo,
            Direccion = cliente.Direccion,
            FechaRegistro = DateTime.Now
        };

        return _clienteRepository.Insertar(entidad);
    }

    public void Actualizar(ClienteDto cliente)
    {
        var entidad = new Cliente
        {
            ClienteId = cliente.ClienteId,
            TipoDocumento = cliente.TipoDocumento,
            NumeroDocumento = cliente.NumeroDocumento,
            Nombres = cliente.Nombres,
            Apellidos = cliente.Apellidos,
            Telefono = cliente.Telefono,
            Correo = cliente.Correo,
            Direccion = cliente.Direccion
        };

        _clienteRepository.Actualizar(entidad);
    }

    public void Eliminar(int clienteId)
    {
        _clienteRepository.Eliminar(clienteId);
    }
}
