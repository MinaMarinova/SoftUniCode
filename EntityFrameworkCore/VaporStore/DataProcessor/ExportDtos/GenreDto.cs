using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace VaporStore.DataProcessor.ExportDtos
{
    public class GenreDto
    {
        public int Id { get; set; }

        [JsonProperty("Genre")]
        public string Name { get; set; }

        public List<GameDto> Games { get; set; } = new List<GameDto>();

        public int TotalPlayers { get; set; }
    }
}
