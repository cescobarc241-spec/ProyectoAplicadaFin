using HotelGestion.Domain.Entities;
using HotelGestion.Domain.Interfaces;
using HotelGestion.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace HotelGestion.Infrastructure.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly DbConnectionFactory _connectionFactory;

    public ClienteRepository(DbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public List<Cliente> ObtenerTodos()
    {
        var clientes = new List<Cliente>();

        using var connection = _connectionFactory.CreateConnection();

        connection.Open();

        const string sql = """
        SELECT
            ClienteId,
            TipoDocumento,
            NumeroDocumento,
            Nombres,
            Apellidos,
            Telefono,
            Correo,
            Direccion,
            FechaRegistro
        FROM Cliente;
        """;

        using var command = new SqlCommand(sql, connection);

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var cliente = new Cliente
            {
                ClienteId = reader.GetInt32(reader.GetOrdinal("ClienteId")),
                TipoDocumento = reader.GetString(reader.GetOrdinal("TipoDocumento")),
                NumeroDocumento = reader.GetString(reader.GetOrdinal("NumeroDocumento")),
                Nombres = reader.GetString(reader.GetOrdinal("Nombres")),
                Apellidos = reader.GetString(reader.GetOrdinal("Apellidos")),
                Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Telefono")),
                Correo = reader.IsDBNull(reader.GetOrdinal("Correo"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Correo")),
                Direccion = reader.IsDBNull(reader.GetOrdinal("Direccion"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Direccion")),
                FechaRegistro = reader.GetDateTime(reader.GetOrdinal("FechaRegistro"))
            };

            clientes.Add(cliente);
        }

        return clientes;
    }

    public Cliente? ObtenerPorId(int clienteId)
    {
        using var connection = _connectionFactory.CreateConnection();

        connection.Open();

        const string sql = """
        SELECT
            ClienteId,
            TipoDocumento,
            NumeroDocumento,
            Nombres,
            Apellidos,
            Telefono,
            Correo,
            Direccion,
            FechaRegistro
        FROM Cliente
        WHERE ClienteId = @ClienteId;
        """;

        using var command = new SqlCommand(sql, connection);

        command.Parameters.Add("@ClienteId", SqlDbType.Int).Value = clienteId;

        using var reader = command.ExecuteReader();

        if (!reader.Read())
        {
            return null;
        }

        return new Cliente
        {
            ClienteId = reader.GetInt32(reader.GetOrdinal("ClienteId")),
            TipoDocumento = reader.GetString(reader.GetOrdinal("TipoDocumento")),
            NumeroDocumento = reader.GetString(reader.GetOrdinal("NumeroDocumento")),
            Nombres = reader.GetString(reader.GetOrdinal("Nombres")),
            Apellidos = reader.GetString(reader.GetOrdinal("Apellidos")),
            Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono"))
                ? null
                : reader.GetString(reader.GetOrdinal("Telefono")),
            Correo = reader.IsDBNull(reader.GetOrdinal("Correo"))
                ? null
                : reader.GetString(reader.GetOrdinal("Correo")),
            Direccion = reader.IsDBNull(reader.GetOrdinal("Direccion"))
                ? null
                : reader.GetString(reader.GetOrdinal("Direccion")),
            FechaRegistro = reader.GetDateTime(reader.GetOrdinal("FechaRegistro"))
        };
    }

    public int Insertar(Cliente cliente)
    {
        using var connection = _connectionFactory.CreateConnection();

        connection.Open();

        const string sql = """
        INSERT INTO Cliente
        (
            TipoDocumento,
            NumeroDocumento,
            Nombres,
            Apellidos,
            Telefono,
            Correo,
            Direccion,
            FechaRegistro
        )
        OUTPUT INSERTED.ClienteId
        VALUES
        (
            @TipoDocumento,
            @NumeroDocumento,
            @Nombres,
            @Apellidos,
            @Telefono,
            @Correo,
            @Direccion,
            @FechaRegistro
        );
        """;

        using var command = new SqlCommand(sql, connection);

        command.Parameters.Add("@TipoDocumento", SqlDbType.VarChar, 20)
            .Value = cliente.TipoDocumento;

        command.Parameters.Add("@NumeroDocumento", SqlDbType.VarChar, 20)
            .Value = cliente.NumeroDocumento;

        command.Parameters.Add("@Nombres", SqlDbType.VarChar, 100)
            .Value = cliente.Nombres;

        command.Parameters.Add("@Apellidos", SqlDbType.VarChar, 100)
            .Value = cliente.Apellidos;

        command.Parameters.Add("@Telefono", SqlDbType.VarChar, 20)
            .Value = (object?)cliente.Telefono ?? DBNull.Value;

        command.Parameters.Add("@Correo", SqlDbType.VarChar, 150)
            .Value = (object?)cliente.Correo ?? DBNull.Value;

        command.Parameters.Add("@Direccion", SqlDbType.VarChar, 200)
            .Value = (object?)cliente.Direccion ?? DBNull.Value;

        command.Parameters.Add("@FechaRegistro", SqlDbType.DateTime2)
            .Value = cliente.FechaRegistro;

        return (int)command.ExecuteScalar()!;
    }

    public void Actualizar(Cliente cliente)
    {
        using var connection = _connectionFactory.CreateConnection();

        connection.Open();

        const string sql = """
        UPDATE Cliente
        SET
            TipoDocumento = @TipoDocumento,
            NumeroDocumento = @NumeroDocumento,
            Nombres = @Nombres,
            Apellidos = @Apellidos,
            Telefono = @Telefono,
            Correo = @Correo,
            Direccion = @Direccion
        WHERE ClienteId = @ClienteId;
        """;

        using var command = new SqlCommand(sql, connection);

        command.Parameters.Add("@TipoDocumento", SqlDbType.VarChar, 20)
            .Value = cliente.TipoDocumento;

        command.Parameters.Add("@NumeroDocumento", SqlDbType.VarChar, 20)
            .Value = cliente.NumeroDocumento;

        command.Parameters.Add("@Nombres", SqlDbType.VarChar, 100)
            .Value = cliente.Nombres;

        command.Parameters.Add("@Apellidos", SqlDbType.VarChar, 100)
            .Value = cliente.Apellidos;

        command.Parameters.Add("@Telefono", SqlDbType.VarChar, 20)
            .Value = (object?)cliente.Telefono ?? DBNull.Value;

        command.Parameters.Add("@Correo", SqlDbType.VarChar, 150)
            .Value = (object?)cliente.Correo ?? DBNull.Value;

        command.Parameters.Add("@Direccion", SqlDbType.VarChar, 200)
            .Value = (object?)cliente.Direccion ?? DBNull.Value;

        command.Parameters.Add("@ClienteId", SqlDbType.Int)
            .Value = cliente.ClienteId;

        command.ExecuteNonQuery();
    }

    public void Eliminar(int clienteId)
    {
        using var connection = _connectionFactory.CreateConnection();

        connection.Open();

        const string sql = """
        DELETE FROM Cliente
        WHERE ClienteId = @ClienteId;
        """;

        using var command = new SqlCommand(sql, connection);

        command.Parameters.Add("@ClienteId", SqlDbType.Int)
            .Value = clienteId;

        command.ExecuteNonQuery();
    }
}
