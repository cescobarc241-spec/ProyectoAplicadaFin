using HotelGestion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Domain.Interfaces
{
    public interface ITipoHabitacionRepository
    {
        List<TipoHabitacion> ObtenerTodos();

        TipoHabitacion? ObtenerPorId(int tipoHabitacionId);

        int Insertar(TipoHabitacion tipoHabitacion);

        void Actualizar(TipoHabitacion tipoHabitacion);

        void Eliminar(int tipoHabitacionId);
    }
}
