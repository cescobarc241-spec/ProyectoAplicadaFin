using HotelGestion.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Application.Interfaces
{
    public interface IHabitacionService
    {
        List<HabitacionDto> ObtenerTodos();

        HabitacionDto? ObtenerPorId(int habitacionId);

        int Registrar(HabitacionDto habitacion);

        void Actualizar(HabitacionDto habitacion);

        void Eliminar(int habitacionId);
    }
}
