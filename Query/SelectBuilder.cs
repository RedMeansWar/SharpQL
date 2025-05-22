using System.Text;

namespace SharpQL.Query;

public class SelectBuilder
{
    internal string _table = string.Empty;
    internal readonly List<string> _columns = new();
    internal readonly List<string> _conditions = new();
    internal readonly Dictionary<string, object> _parameters = new();
    internal string? _orderBy;
    internal int? _limit;

    /// <summary>
    /// Gets the SQL query string that has been built based on the defined
    /// columns, table, conditions, and other query components. This property
    /// is only populated after invoking the <c>Build()</c> method on the
    /// <c>SelectBuilder</c> instance.
    /// </summary>
    public string Sql { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the collection of parameters utilized in the SQL query, where
    /// each key-value pair represents a parameter name and its corresponding value.
    /// This property is populated after invoking the <c>Build()</c> method on the
    /// <c>SelectBuilder</c> instance.
    /// </summary>
    public Dictionary<string, object> Parameters { get; private set; } = new();

    /// <summary>
    /// Specifies the columns to be selected in the SQL query.
    /// </summary>
    /// <param name="columns">An array of column names to include in the SELECT statement.</param>
    /// <returns>A reference to the current <see cref="SelectBuilder"/> instance for method chaining.</returns>
    public SelectBuilder Select(params string[] columns)
    {
        _columns.AddRange(columns);
        return this;
    }

    /// <summary>
    /// Selects all columns in the SQL query.
    /// </summary>
    /// <returns>A reference to the current <see cref="SelectBuilder"/> instance for method chaining.</returns>
    public SelectBuilder SelectAll()
    {
        _columns.Clear();
        _columns.Add("*");
        return this;
    }

    /// <summary>
    /// Specifies the table to be queried in the SQL statement.
    /// </summary>
    /// <param name="table">The name of the table to query.</param>
    /// <returns>A reference to the current <see cref="SelectBuilder"/> instance for method chaining.</returns>
    public SelectBuilder From(string table)
    {
        _table = table;
        return this;
    }

    /// <summary>
    /// Adds a WHERE clause to the SQL query to filter results based on the specified column and value.
    /// </summary>
    /// <param name="column">The name of the column to evaluate in the WHERE condition.</param>
    /// <param name="value">The value to match against the specified column.</param>
    /// <returns>A reference to the current <see cref="SelectBuilder"/> instance for method chaining.</returns>
    public SelectBuilder Where(string column, object value)
    {
        var paramName = $"@{column}_{_parameters.Count}";
        _conditions.Add($"{column} = {paramName}");
        _parameters[paramName] = value;
        return this;
    }

    /// <summary>
    /// Specifies the column by which to order the results of the SQL query.
    /// </summary>
    /// <param name="column">The name of the column to order by.</param>
    /// <param name="ascending">A boolean value indicating whether the order should be ascending (true) or descending (false). Default is true (ascending).</param>
    /// <returns>A reference to the current <see cref="SelectBuilder"/> instance for method chaining.</returns>
    public SelectBuilder OrderBy(string column, bool ascending = true)
    {
        _orderBy = $"{column} {(ascending ? "ASC" : "DESC")}";
        return this;
    }

    /// <summary>
    /// Limits the number of rows returned in the SQL query.
    /// </summary>
    /// <param name="count">The maximum number of rows to return.</param>
    /// <returns>A reference to the current <see cref="SelectBuilder"/> instance for method chaining.</returns>
    public SelectBuilder Limit(int count)
    {
        _limit = count;
        return this;
    }

    /// <summary>
    /// Constructs the SQL query string and finalizes the query definition.
    /// </summary>
    /// <returns>A reference to the current <see cref="SelectBuilder"/> instance containing the finalized query string and parameters.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the table name has not been specified.</exception>
    public SelectBuilder Build()
    {
        if (string.IsNullOrWhiteSpace(_table))
            throw new InvalidOperationException("Table name must be specified with .From()");

        var sb = new StringBuilder();
        sb.Append("SELECT ");
        sb.Append(_columns.Count > 0 ? string.Join(", ", _columns) : "*");
        sb.Append(" FROM ").Append(_table);

        if (_conditions.Count > 0) sb.Append(" WHERE ").Append(string.Join(" AND ", _conditions));
        if (!string.IsNullOrEmpty(_orderBy)) sb.Append(" ORDER BY ").Append(_orderBy);
        if (_limit.HasValue) sb.Append(" LIMIT ").Append(_limit.Value);

        Sql = sb.ToString();
        Parameters = new Dictionary<string, object>(_parameters);
        return this;
    }
}