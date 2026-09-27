using System;
using System.Collections.Generic;
using System.Text;
using HotelGestion.Application.DTOs;
using HotelGestion.Application.Interfaces;

namespace HotelGestion.Application.UseCases;

public class CheckOutService : ICheckOutService
{
    private readonly ICheckOutRepository _checkOutRepository;

    public CheckOutService(ICheckOutRepository checkOutRepository)
    {
        _checkOutRepository = checkOutRepository;
    }

    public void Ejecutar(CheckOutDto checkOut)
    {
        if (checkOut.EstanciaId <= 0)
        {
            throw new ArgumentException(
                "La estancia indicada no es válida.");
        }

        if (string.IsNullOrWhiteSpace(checkOut.MetodoPago))
        {
            throw new ArgumentException(
                "Debe indicar el método de pago.");
        }

        _checkOutRepository.Ejecutar(checkOut);
    }
}