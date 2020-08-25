using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Polly;
using Polly.Retry;

namespace Garage.Polly.Extensions.Dapper
{
    /// <summary>
    /// Includes a collection of extension methods for <see cref="Dapper"/>.
    /// </summary>
    public static class DapperExtensions
    {
        private const int RetryCount = 4;
        private static readonly Random Random = new Random();
        private static readonly AsyncRetryPolicy RetryPolicy = Policy
            .Handle<SqlException>(SqlServerTransientExceptionDetector.ShouldRetryOn)
            .Or<TimeoutException>()
            .OrInner<Win32Exception>(SqlServerTransientExceptionDetector.ShouldRetryOn)
            .WaitAndRetryAsync(
                RetryCount,
                currentRetryNumber => TimeSpan.FromSeconds(Math.Pow(1.5, currentRetryNumber - 1)) + TimeSpan.FromMilliseconds(Random.Next(0, 100)),
                (currentException, currentSleepDuration, currentRetryNumber, currentContext) =>
                {
#if DEBUG
                    Debug.WriteLine($"=== Attempt {currentRetryNumber} ===");
                    Debug.WriteLine(nameof(currentException) + ": " + currentException);
                    Debug.WriteLine(nameof(currentContext) + ": " + currentContext);
                    Debug.WriteLine(nameof(currentSleepDuration) + ": " + currentSleepDuration);
#endif
                });

        /// <summary>
        /// Wraps ExecuteAsync with retry policy.
        /// </summary>
        /// <param name="cnn">The db connection</param>
        /// <param name="sql">The sql query</param>
        /// <param name="param">The sql query parameters</param>
        /// <param name="transaction">The db transaction</param>
        /// <param name="commandTimeout">The command timeout value</param>
        /// <param name="commandType">The command type</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public static Task<int> ExecuteAsyncWithRetry(
            this IDbConnection cnn,
            string sql,
            object param = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null) => RetryPolicy.ExecuteAsync(() => cnn.ExecuteAsync(sql, param, transaction, commandTimeout, commandType));

        /// <summary>
        /// Wraps QueryAsync with retry policy.
        /// </summary>
        /// <typeparam name="T">The generic type of returning object</typeparam>
        /// <param name="cnn">The db connection</param>
        /// <param name="sql">The sql query</param>
        /// <param name="param">The sql query parameters</param>
        /// <param name="transaction">The db transaction</param>
        /// <param name="commandTimeout">The command timeout value</param>
        /// <param name="commandType">The command type</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public static Task<IEnumerable<T>> QueryAsyncWithRetry<T>(
            this IDbConnection cnn,
            string sql,
            object param = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null) => RetryPolicy.ExecuteAsync(() => cnn.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType));

        /// <summary>
        /// Wraps QueryFirstOrDefaultAsync with retry policy.
        /// </summary>
        /// <typeparam name="T">The generic type of returning object</typeparam>
        /// <param name="cnn">The db connection</param>
        /// <param name="sql">The sql query</param>
        /// <param name="param">The sql query parameters</param>
        /// <param name="transaction">The db transaction</param>
        /// <param name="commandTimeout">The command timeout value</param>
        /// <param name="commandType">The command type</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public static Task<T> QueryFirstOrDefaultAsyncWithRetry<T>(
            this IDbConnection cnn,
            string sql,
            object param = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null) => RetryPolicy.ExecuteAsync(() => cnn.QueryFirstOrDefaultAsync<T>(sql, param, transaction, commandTimeout, commandType));

        /// <summary>
        /// Wraps QueryMultipleAsync with retry policy.
        /// </summary>
        /// <param name="cnn">The db connection</param>
        /// <param name="sql">The sql query</param>
        /// <param name="param">The sql query parameters</param>
        /// <param name="transaction">The db transaction</param>
        /// <param name="commandTimeout">The command timeout value</param>
        /// <param name="commandType">The command type</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public static Task<SqlMapper.GridReader> QueryMultipleAsyncWithRetry(
            this IDbConnection cnn,
            string sql,
            object param = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null) => RetryPolicy.ExecuteAsync(() => cnn.QueryMultipleAsync(sql, param, transaction, commandTimeout, commandType));
    }
}
