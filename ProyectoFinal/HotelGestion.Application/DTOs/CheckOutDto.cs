    using System;
    using System.Collections.Generic;
    using System.Text;

    namespace HotelGestion.Application.DTOs
    {
        public class CheckOutDto
        {
            public int EstanciaId { get; set; }
            public string MetodoPago { get; set; } = string.Empty;
        
            // SOLO PARA LA PRUEBA DE ROLLBACK
            //public bool ForzarError { get; set; }
        }
    }
