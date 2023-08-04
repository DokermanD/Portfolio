using System.Collections.Generic;
using Newtonsoft.Json;
using UnstackerFix.MarkerScript;

namespace UnstackerFix
{
    public class ModelJson
    {
        [JsonProperty("marker")] public Marker Marker { get; set; }

        [JsonProperty("action")] public List<Action> Action { get; set; }
    }

    public class Action
    {
        [JsonProperty("button")] public string Button { get; set; }

        [JsonProperty("action")] public string Actions { get; set; }

        [JsonProperty("args")] public string Args { get; set; }
    }

    public class Body
    {
        [JsonProperty("markerId")] public string MarkerId { get; set; }

        [JsonProperty("accuracy")] public int Accuracy { get; set; }

        [JsonProperty("searchArea")] public SearchArea SearchArea { get; set; }
    }

    public class Marker
    {
        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("body")] public Body Body { get; set; }
    }

    public class SearchArea
    {
        [JsonProperty("x")] public int X { get; set; }

        [JsonProperty("y")] public int Y { get; set; }

        [JsonProperty("width")] public int Width { get; set; }

        [JsonProperty("height")] public int Height { get; set; }

        public static implicit operator SearchArea(SearchAreaSkript v)
        {
            throw new System.NotImplementedException();
        }
    }
}