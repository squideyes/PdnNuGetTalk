using System.Text.Json;

namespace BogusDemo;

public static class MiscExtnders
{
    public static void Dump(this object instance) =>
        Console.WriteLine(instance.DumpJson());

    public static string DumpJson(this object instance) =>
        JsonSerializer.Serialize(instance);
}
