using HotelGestion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Domain.Interfaces
{
    public interface IConsumoMinibarRepository
    {
        List<ConsumoMinibar> ObtenerTodos();

        ConsumoMinibar? ObtenerPorId(int consumoMinibarId);

        int Insertar(ConsumoMinibar consumoMinibar);

        void Actualizar(ConsumoMinibar consumoMinibar);

        void Eliminar(int consumoMinibarId);
    }
}
