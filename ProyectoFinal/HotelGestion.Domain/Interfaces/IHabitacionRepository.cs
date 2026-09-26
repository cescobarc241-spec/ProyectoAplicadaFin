using HotelGestion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Domain.Interfaces
{
    public interface IHabitacionRepository
    {
        List<Habitacion> ObtenerTodos();

        Habitacion? ObtenerPorId(int habitacionId);

        int Insertar(Habitacion habitacion);

        void Actualizar(Habitacion habitacion);

        void Eliminar(int habitacionId);
    }
}
