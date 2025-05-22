using System.Data.Common;

#if NET9_0_OR_GREATER
using Microsoft.Data.SqlClient;
#else
using System.Data.SqlClient;
#endif

using SharpQL.Connection;

namespace SharpQL.Execution;

public static class QueryExecutor
{
    /// <summary>
    /// Executes an SQL command against a database connection and returns the number of rows affected.
    /// </summary>
    /// <param name="sql">The SQL command to execute against the database.</param>
    /// <param name="parameters">An optional dictionary containing parameter names and their corresponding values to be included in the SQL command.</param>
    /// <returns>The number of rows affected by the command.</returns>
    public static int ExecuteNonQuery(string sql, Dictionary<string, object>? parameters = null)
    {
        using var connection = Db.Create();
        using var command = CreateCommand(connection, sql, parameters);
        connection.Open();
        return command.ExecuteNonQuery();
    }

    /// <summary>
    /// Executes an SQL command asynchronously against a database connection and returns the number of rows affected.
    /// </summary>
    /// <param name="sql">The SQL command to execute against the database.</param>
    /// <param name="parameters">An optional dictionary containing parameter names and their corresponding values to be included in the SQL command.</param>
    /// <returns>A task representing the asynchronous operation, containing the number of rows affected by the command.</returns>
    public static async Task<int> ExecuteNonQueryAsync(string sql, Dictionary<string, object>? parameters = null)
    {
        using var connection = await Db.CreateAsync();
        using var command = CreateCommand(connection, sql, parameters);
        return await command.ExecuteNonQueryAsync();
    }

    /// <summary>
    /// Executes an SQL query and retrieves the first column of the first row in the result set returned by the query.
    /// Additional rows and columns are ignored.
    /// </summary>
    /// <param name="sql">The SQL query to execute against the database.</param>
    /// <param name="parameters">An optional dictionary containing parameter names and their corresponding values to be included in the query.</param>
    /// <returns>The first column of the first row in the result set, or null if the result set is empty.</returns>
    public static object? ExecuteScalar(string sql, Dictionary<string, object>? parameters = null)
    {
        using var connection = Db.Create();
        using var command = CreateCommand(connection, sql, parameters);
        connection.Open();
        return command.ExecuteScalar();
    }

    /// <summary>
    /// Asynchronously executes an SQL query and retrieves the first column of the first row in the result set returned by the query.
    /// Additional rows and columns are ignored.
    /// </summary>
    /// <param name="sql">The SQL query to execute against the database.</param>
    /// <param name="parameters">An optional dictionary containing parameter names and their corresponding values to be included in the query.</param>
    /// <returns>A task representing the asynchronous operation, containing the first column of the first row in the result set, or null if the result set is empty.</returns>
    public static async Task<object?> ExecuteScalarAsync(string sql, Dictionary<string, object>? parameters = null)
    {
        using var connection = await Db.CreateAsync();
        using var command = CreateCommand(connection, sql, parameters);
        
        return await command.ExecuteScalarAsync();
    }

    /// <summary>
    /// Executes an SQL query and retrieves the results as a list of dictionaries, where each dictionary represents a row and maps column names to their corresponding values.
    /// </summary>
    /// <param name="sql">The SQL query to execute against the database.</param>
    /// <param name="parameters">An optional dictionary containing parameter names and their corresponding values to be included in the query.</param>
    /// <returns>A list of dictionaries that represent the result set of the query.</returns>
    public static List<Dictionary<string, object>> ExecuteReader(string sql, Dictionary<string, object>? parameters = null)
    {
        using var connection = Db.Create();
        using var command = CreateCommand(connection, sql, parameters);
        connection.Open();
        
        using var reader = command.ExecuteReader();
        return ReadAll(reader);
    }

    /// <summary>
    /// Asynchronously executes an SQL query and retrieves the results as a list of dictionaries, where each dictionary represents a row and maps column names to their corresponding values.
    /// </summary>
    /// <param name="sql">The SQL query to execute against the database.</param>
    /// <param name="parameters">An optional dictionary containing parameter names and their corresponding values to be included in the query.</param>
    /// <returns>A task representing the asynchronous operation, containing a list of dictionaries that represent the result set of the query.</returns>
    public static async Task<List<Dictionary<string, object>>> ExecuteReaderAsync(string sql, Dictionary<string, object>? parameters = null)
    {
        using var connection = await Db.CreateAsync();
        using var command = CreateCommand(connection, sql, parameters);
        using var reader = await command.ExecuteReaderAsync();

        return await ReadAllAsync(reader);
    }

    /// <summary>
    /// Creates a database command with the specified SQL query and optional parameters for a given database connection.
    /// </summary>
    /// <param name="connection">The database connection used to create the command.</param>
    /// <param name="sql">The SQL query to be executed by the command.</param>
    /// <param name="parameters">An optional dictionary containing parameter names and their values to be added to the command.</param>
    /// <returns>A database command configured with the provided SQL query and parameters.</returns>
    private static DbCommand CreateCommand(DbConnection connection, string sql, Dictionary<string, object>? parameters)
    {
        var command = connection.CreateCommand();
        command.CommandText = sql;

        if (parameters is null) return command;
        
        foreach (var param in parameters)
        {
            var dbParam = command.CreateParameter();
            dbParam.ParameterName = param.Key;
            dbParam.Value = param.Value;
            command.Parameters.Add(dbParam);
        }

        return command;
    }

    /// <summary>
    /// Reads all rows from the provided DbDataReader and converts them into a list of dictionaries,
    /// where each dictionary represents a row with column names as keys and their respective values.
    /// </summary>
    /// <param name="reader">The DbDataReader instance to read data from.</param>
    /// <returns>A list of dictionaries, each representing a row of data, with column names as keys and their corresponding values.</returns>
    private static List<Dictionary<string, object>> ReadAll(DbDataReader reader)
    {
        var results = new List<Dictionary<string, object>>();
        while (reader.Read())
        {
            var row = new Dictionary<string, object>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                row[reader.GetName(i)] = reader.IsDBNull(i) ? null! : reader.GetValue(i);
            }
            
            results.Add(row);
        }
        
        return results;
    }

    /// <summary>
    /// Reads all rows from the provided DbDataReader and converts them into a list of dictionaries,
    /// where each dictionary represents a row with column names as keys and their respective values.
    /// </summary>
    /// <param name="reader">The DbDataReader instance to read data from.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a list of dictionaries, each representing a row of data.</returns>
    private static async Task<List<Dictionary<string, object>>> ReadAllAsync(DbDataReader reader)
    {
        var result = new List<Dictionary<string, object>>();
        while (await reader.ReadAsync())
        {
            var row = new Dictionary<string, object>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                row[reader.GetName(i)] = await reader.IsDBNullAsync(i) ? null! : reader.GetValue(i);
            }
            
            result.Add(row);
        }

        return result;
    }
}