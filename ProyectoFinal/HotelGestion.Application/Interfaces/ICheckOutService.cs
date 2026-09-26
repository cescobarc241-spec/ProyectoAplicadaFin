using HotelGestion.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Application.Interfaces
{
    public interface ICheckOutService
    {
        void Ejecutar(CheckOutDto checkOut);
    }
}
