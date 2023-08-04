using Newtonsoft.Json;

namespace UnstackerFix.MarkerScript
{
    public class ModelJsonScript
    {
        [JsonProperty("markerId")] public string MarkerId { get; set; }

        [JsonProperty("searchArea")] public SearchAreaSkript SearchAreaSkript { get; set; }

        [JsonProperty("accuracy")] public int Accuracy { get; set; }
    }

    public class SearchAreaSkript
    {
        [JsonProperty("x")] public int X { get; set; }

        [JsonProperty("y")] public int Y { get; set; }

        [JsonProperty("width")] public int Width { get; set; }

        [JsonProperty("height")] public int Height { get; set; }
    }
}