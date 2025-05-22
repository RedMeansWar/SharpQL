namespace SharpQL.Schemas;

[Flags]
public enum ColumnConstraint
{
    None = 0,
    PrimaryKey = 1 << 0,
    AutoIncrement = 1 << 1,
    NotNull = 1 << 2,
    Unique = 1 << 3
}