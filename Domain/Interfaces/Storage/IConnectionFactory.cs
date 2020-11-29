using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Domain.Interfaces.Storage
{
    public interface IConnectionFactory
    {
        IDbConnection GetDbConnection();
    }
}
