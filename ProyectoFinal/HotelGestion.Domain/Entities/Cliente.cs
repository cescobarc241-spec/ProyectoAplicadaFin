using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Domain.Entities
{
    public class Cliente
    {
        public int ClienteId { get; set; }

        public string TipoDocumento { get; set; } = string.Empty;

        public string NumeroDocumento { get; set; } = string.Empty;

        public string Nombres { get; set; } = string.Empty;

        public string Apellidos { get; set; } = string.Empty;

        public string? Telefono { get; set; }

        public string? Correo { get; set; }

        public string? Direccion { get; set; }

        public DateTime FechaRegistro { get; set; }
    }
}
