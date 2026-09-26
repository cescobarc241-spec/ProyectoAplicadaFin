using HotelGestion.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Application.Interfaces
{
    public interface IEstanciaService
    {
        List<EstanciaDto> ObtenerTodos();

        EstanciaDto? ObtenerPorId(int estanciaId);

        int Registrar(EstanciaDto estancia);

        void Actualizar(EstanciaDto estancia);

        void Eliminar(int estanciaId);
    }
}
