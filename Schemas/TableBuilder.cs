using System.Text;
using SharpQL.Dialects;
using SharpQL.Models;

namespace SharpQL.Schemas;

public class TableBuilder
{
    internal readonly string _tableName;
    internal readonly List<ColumnDefinition> _columns = new();
    
    public TableBuilder(string tableName) => _tableName = tableName;

    /// <summary>
    /// Adds a column definition to the table being built, including the column's name, type, and constraints.
    /// </summary>
    /// <param name="name">The name of the column to add.</param>
    /// <param name="type">The SQL data type of the column.</param>
    /// <param name="constraints">The constraints to apply to the column, such as PrimaryKey, NotNull, or Unique. Defaults to None.</param>
    /// <returns>The current instance of the <see cref="TableBuilder"/> to allow for method chaining.</returns>
    public TableBuilder AddColumn(string name, SqlType type, ColumnConstraint constraints = ColumnConstraint.None)
    {
        _columns.Add(new ColumnDefinition(name, type, constraints));
        return this;
    }

    /// <summary>
    /// Builds and returns the SQL command string for creating a table,
    /// including the table name and column definitions.
    /// </summary>
    /// <returns>A string containing the SQL command to create the table.</returns>
    public string Build()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"CREATE TABLE IF NOT EXISTS {_tableName} (");
        
        for (int i = 0; i < _columns.Count; i++)
        {
            var column = _columns[i];
            sb.Append("    ").Append(column.ToSql());
            
            if (i < _columns.Count - 1) sb.AppendLine(",");
            sb.AppendLine();
        }
        
        sb.AppendLine(");");
        return sb.ToString();
    }
}