using System;
using System.Collections.Generic;
using System.Text;
using HotelGestion.Application.DTOs;

namespace HotelGestion.Application.Interfaces;

public interface ICheckInService
{
    int Ejecutar(CheckInDto checkIn);
}
