using HotelGestion.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Application.Interfaces
{
    public interface IClienteService
    {
        List<ClienteDto> ObtenerTodos();

        ClienteDto? ObtenerPorId(int clienteId);

        int Registrar(ClienteDto cliente);

        void Actualizar(ClienteDto cliente);

        void Eliminar(int clienteId);
    }
}
