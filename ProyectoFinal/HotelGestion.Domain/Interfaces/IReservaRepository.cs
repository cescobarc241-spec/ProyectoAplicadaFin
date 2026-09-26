using HotelGestion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Domain.Interfaces
{
    public interface IReservaRepository
    {
        List<Reserva> ObtenerTodos();

        Reserva? ObtenerPorId(int reservaId);

        int Insertar(Reserva reserva);

        void Actualizar(Reserva reserva);

        void Eliminar(int reservaId);
    }
}
