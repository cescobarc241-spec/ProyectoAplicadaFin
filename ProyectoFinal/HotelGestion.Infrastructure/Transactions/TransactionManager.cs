using HotelGestion.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Infrastructure.Transactions;

public class TransactionManager : ITransactionManager, IDisposable
{
    private readonly SqlConnection _connection;

    public SqlConnection Connection => _connection;

    public SqlTransaction Transaction { get; }

    public TransactionManager(DbConnectionFactory connectionFactory)
    {
        _connection = connectionFactory.CreateConnection();

        _connection.Open();

        Transaction = _connection.BeginTransaction();
    }

    public void Commit()
    {
        Transaction.Commit();
        Dispose();
    }

    public void Rollback()
    {
        Transaction.Rollback();
        Dispose();
    }

    public void Dispose()
    {
        Transaction.Dispose();
        _connection.Close();
        _connection.Dispose();
    }
}