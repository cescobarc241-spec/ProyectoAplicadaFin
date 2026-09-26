using HotelGestion.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Application.Interfaces
{
    public interface IReservaService
    {
        List<ReservaDto> ObtenerTodos();

        ReservaDto? ObtenerPorId(int reservaId);

        int Registrar(ReservaDto reserva);

        void Actualizar(ReservaDto reserva);

        void Eliminar(int reservaId);
    }
}
