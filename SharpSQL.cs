using System;
using SharpQL.Connection;
using SharpQL.Dialects;
using SharpQL.Schemas;
#if NET9_0_OR_GREATER
using Microsoft.Data.SqlClient;
#else
using System.Data.SqlClient;
#endif

namespace SharpQL;

/// <summary>
/// Main entry point for the SharpSQL library.
/// Provides access to connection, schema building, and query utilities.
/// </summary>
public static class SharpSQL
{
    /// <summary>
    /// Establishes a connection to the database using the provided connection string.
    /// </summary>
    /// <param name="connectionString">The connection string used to establish the connection.</param>
    public static void Connect(string connectionString) => Db.Connect(connectionString);

    /// <summary>
    /// Closes the currently open database connection if one exists.
    /// </summary>
    public static void Close() => Db.Close();

    /// <summary>
    /// Indicates whether there is an active and open connection to the database.
    /// </summary>
    /// <returns>
    /// Returns true if the database connection is currently open; otherwise, false.
    /// </returns>
    public static bool IsConnected => Db.IsOpen;

    /// <summary>
    /// Creates a new table builder for defining a database table with the specified name.
    /// </summary>
    /// <param name="tableName">The name of the table to be created.</param>
    /// <returns>A <see cref="TableBuilder"/> instance for defining the table's columns and constraints.</returns>
    public static TableBuilder CreateTable(string tableName) => new(tableName);
}
