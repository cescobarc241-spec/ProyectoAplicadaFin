using System;
using System.Collections.Generic;
using System.Text;
using HotelGestion.Domain.Entities;

namespace HotelGestion.Domain.Interfaces
{
    public interface IClienteRepository
    {
        List<Cliente> ObtenerTodos();

        Cliente? ObtenerPorId(int clienteId);

        int Insertar(Cliente cliente);

        void Actualizar(Cliente cliente);

        void Eliminar(int clienteId);
    }
}
