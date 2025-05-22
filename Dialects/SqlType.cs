namespace SharpQL.Dialects;

public enum SqlType
{
    Integer,
    Text,
    Varchar,
    Boolean,
    DateTime,
    Decimal,
    Float,
    Double,
    Blob
}

/// <summary>
/// Provides extension methods for the SqlType enum.
/// </summary>
public static class SqlTypeExtensions
{
    /// <summary>
    /// Converts the SqlType enum value to its corresponding SQL type as a string.
    /// </summary>
    /// <param name="type">The SqlType enum value to convert.</param>
    /// <returns>A string representation of the SQL data type corresponding to the specified SqlType.</returns>
    /// <exception cref="NotImplementedException">Thrown when the specified SqlType is not supported.</exception>
    public static string ToSql(this SqlType type) => type switch
    {
        SqlType.Integer => "INTEGER",
        SqlType.Text => "TEXT",
        SqlType.Varchar => "VARCHAR(255)",
        SqlType.Boolean => "BOOLEAN",
        SqlType.DateTime => "DATETIME",
        SqlType.Decimal => "DECIMAL",
        SqlType.Float => "FLOAT",
        SqlType.Double => "DOUBLE",
        SqlType.Blob => "BLOB",
        _ => throw new NotImplementedException($"Unsupported type: {type}")
    };
}