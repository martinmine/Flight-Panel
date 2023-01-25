using System.Text.Json;

namespace FlightListener;

public static class JsonElementExtensions
{
    public static string? GetNullableString(this JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.Null)
            return default;
        
        return element.GetString();
    }
    
    public static int GetNullableInt32(this JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.Null)
            return default;
        
        return element.GetInt32();
    }
    
    public static double GetNullableDouble(this JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.Null)
            return default;
        
        return element.GetDouble();
    }
    
    public static bool GetNullableBoolean(this JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.Null)
            return default;
        
        return element.GetBoolean();
    }
}