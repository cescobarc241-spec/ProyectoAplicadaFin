using HotelGestion.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Application.Interfaces
{
    public interface ITipoHabitacionService
    {
        List<TipoHabitacionDto> ObtenerTodos();

        TipoHabitacionDto? ObtenerPorId(int tipoHabitacionId);

        int Registrar(TipoHabitacionDto tipoHabitacion);

        void Actualizar(TipoHabitacionDto tipoHabitacion);

        void Eliminar(int tipoHabitacionId);
    }
}
