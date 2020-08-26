using System;

namespace Garage.Polly.Extensions.Dapper.Sample.Repositories
{
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        public string Get()
        {
            var connectionString = Environment.GetEnvironmentVariable("ConnectionString");
            return connectionString;
        }
    }
}
