using System.Text.Json.Serialization;

namespace OptionsLib.Options;

public class DummyOptions
{
    public string Name { get; set; } = "Default";

    public int RandomSeed { get; set; } = 12345678;

    public int MinValue { get; set; } = 0;

    public int MaxValue { get; set; } = 100;

    [JsonIgnore]
    public Action<int>? OnNumberGenerated { get; set; }

    public bool ThrowException { get; set; } = false;

    public TimeSpan Delay { get; set; } = new TimeSpan(1, 2, 3);
}
