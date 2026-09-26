using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelGestion.Infrastructure.Transactions
{
    public interface ITransactionManager
    {
        SqlConnection Connection { get; }

        SqlTransaction Transaction { get; }

        void Commit();

        void Rollback();
    }
}
