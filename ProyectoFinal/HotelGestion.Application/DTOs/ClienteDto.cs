using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Application.DTOs
{
    public class ClienteDto
    {
        public int ClienteId { get; set; }

        public string TipoDocumento { get; set; } = string.Empty;

        public string NumeroDocumento { get; set; } = string.Empty;

        public string Nombres { get; set; } = string.Empty;

        public string Apellidos { get; set; } = string.Empty;

        public string? Telefono { get; set; }

        public string? Correo { get; set; }

        public string? Direccion { get; set; }
    }
}
