using System.Text;
using SharpQL.Dialects;
using SharpQL.Schemas;

namespace SharpQL.Models;

public class ColumnDefinition
{
    public string Name { get; set; }
    public SqlType Type { get; set; }
    public ColumnConstraint Constraints { get; set; }
    
    public ColumnDefinition(string name, SqlType type, ColumnConstraint constraints)
    {
        Name = name;
        Type = type;
        Constraints = constraints;
    }

    public string ToSql()
    {
        var sb = new StringBuilder();
        sb.Append($"{Name} {Type.ToSql()}");

        if (Constraints.HasFlag(ColumnConstraint.PrimaryKey)) sb.Append(" PRIMARY KEY");
        if (Constraints.HasFlag(ColumnConstraint.AutoIncrement)) sb.Append(" AUTOINCREMENT");
        if (Constraints.HasFlag(ColumnConstraint.NotNull)) sb.Append(" NOT NULL");
        if (Constraints.HasFlag(ColumnConstraint.Unique)) sb.Append(" UNIQUE");

        return sb.ToString();
    }
}