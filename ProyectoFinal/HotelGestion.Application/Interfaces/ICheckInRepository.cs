using System;
using System.Collections.Generic;
using System.Text;
using HotelGestion.Application.DTOs;

namespace HotelGestion.Application.Interfaces;

public interface ICheckInRepository
{
    int Ejecutar(CheckInDto checkIn);
}
