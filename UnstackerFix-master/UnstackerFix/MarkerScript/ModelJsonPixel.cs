using Newtonsoft.Json;

namespace UnstackerFix.MarkerScript
{
    public class ModelJsonPixel
    {
        [JsonProperty("markerId")] public string markerId { get; set; }

        [JsonProperty("position")] public Position position { get; set; }

        [JsonProperty("color")] public Color color { get; set; }

        [JsonProperty("delta")] public int delta { get; set; }
    }

    public class Color
    {
        [JsonProperty("red")] public int red { get; set; }

        [JsonProperty("green")] public int green { get; set; }

        [JsonProperty("blue")] public int blue { get; set; }
    }

    public class Position
    {
        [JsonProperty("X")] public int X { get; set; }

        [JsonProperty("Y")] public int Y { get; set; }
    }
}