using HotelGestion.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Application.Interfaces
{
    public interface IConsumoMinibarService
    {
        List<ConsumoMinibarDto> ObtenerTodos();

        ConsumoMinibarDto? ObtenerPorId(int consumoMinibarId);

        int Registrar(ConsumoMinibarDto consumo);

        void Actualizar(ConsumoMinibarDto consumo);

        void Eliminar(int consumoMinibarId);
    }
}
