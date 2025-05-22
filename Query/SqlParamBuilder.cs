namespace SharpQL.Query;
    
public static class SqlParamBuilder
{
    /// <summary>
    /// Creates a dictionary from a collection of SQL parameters, where each parameter's name is the key and the value is the corresponding object.
    /// </summary>
    /// <param name="parameters">An array of <see cref="SqlParameter"/> objects to be converted into a dictionary.</param>
    /// <returns>A dictionary containing parameter names as keys and their corresponding values as the dictionary values.</returns>
    public static Dictionary<string, object> Create(params SqlParameter[] parameters)
    {
        var dict = new Dictionary<string, object>();
        foreach (var param in parameters)
        {
            dict[param.Name] = param.Value;
        }
        
        return dict;
    }
    
}
