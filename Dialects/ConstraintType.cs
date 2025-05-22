namespace SharpQL.Dialects;

[Flags]
public enum ConstraintType
{
    None = 0,
    PrimaryKey = 1,
    AutoIncrement = 2,
    NotNull = 4,
    Unique = 8
}