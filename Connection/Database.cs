using System;
using System.Data;
#if NET9_0_OR_GREATER
using Microsoft.Data.SqlClient;
#else
using System.Data.SqlClient;
#endif

namespace SharpQL.Connection;

public static class Db
{
    internal static IDbConnection? _connection;

    /// <summary>
    /// Gets or sets the connection string used to establish a database connection.
    /// </summary>
    /// <value>
    /// A string representing the connection details required to connect to the database.
    /// </value>
    public static string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Gets a value indicating whether the current database connection is open.
    /// </summary>
    /// <value>
    /// Returns true if the database connection is open; otherwise, false.
    /// </value>
    public static bool IsOpen => _connection?.State == ConnectionState.Open;

    /// <summary>
    /// Establishes a connection to the database using the provided connection string.
    /// </summary>
    /// <param name="connectionString">The connection string used to connect to the database.</param>
    public static void Connect(string connectionString)
    {
        if (IsOpen) Close();
        
        ConnectionString = connectionString;
        _connection = new SqlConnection(ConnectionString);
        _connection.Open();
    }

    /// <summary>
    /// Asynchronously establishes a connection to the database using the provided connection string.
    /// </summary>
    /// <param name="connectionString">The connection string used to connect to the database.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task ConnectAsync(string connectionString)
    {
        if (IsOpen) Close();
        ConnectionString = connectionString;
        var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        _connection = connection;
    }

    /// <summary>
    /// Closes the currently open database connection if any exists.
    /// </summary>
    public static void Close()
    {
        if (_connection?.State is not ConnectionState.Closed)
        {
            _connection?.Close();
        }
    }

    /// <summary>
    /// Retrieves the current database connection instance if it exists.
    /// </summary>
    /// <returns>The current database connection instance if established, otherwise null.</returns>
    public static IDbConnection? Get() => _connection;

    /// <summary>
    /// Creates a new instance of a SQL database connection using the configured connection string.
    /// </summary>
    /// <returns>A new SQL database connection instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no connection string has been provided.</exception>
    public static SqlConnection Create()
    {
        if (string.IsNullOrWhiteSpace(ConnectionString)) throw new InvalidOperationException("No connection string has been provided.");
        return new SqlConnection(ConnectionString);
    }

    /// <summary>
    /// Asynchronously creates and opens a new SQL database connection using the configured connection string.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing the newly created and opened SQL database connection.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no connection string has been provided.</exception>
    public static async Task<SqlConnection> CreateAsync()
    {
        if (string.IsNullOrWhiteSpace(ConnectionString)) throw new InvalidOperationException("No connection string has been provided.");
        
        var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        return connection;
    }
}