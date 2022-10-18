// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using System.Text.Json;

namespace BogusDemo;

public static class MiscExtnders
{
    public static void Dump(this object instance) =>
        Console.WriteLine(instance.DumpJson());

    public static string DumpJson(this object instance) =>
        JsonSerializer.Serialize(instance);
}