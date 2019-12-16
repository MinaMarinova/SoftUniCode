using Newtonsoft.Json;

namespace VaporStore.DataProcessor.ExportDtos
{
    public class GameDto
    {
        public int Id { get; set; }

        [JsonProperty("Title")]
        public string Name { get; set; }

        [JsonProperty("Developer")]
        public string DeveloperName { get; set; }

        public string Tags { get; set; }

        public int Players { get; set; }
    }
}
