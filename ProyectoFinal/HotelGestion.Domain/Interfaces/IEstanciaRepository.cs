using HotelGestion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Domain.Interfaces
{
    public interface IEstanciaRepository
    {
        List<Estancia> ObtenerTodos();

        Estancia? ObtenerPorId(int id);

        int Insertar(Estancia entity);

        void Actualizar(Estancia entity);

        void Eliminar(int id);
    }
}
