using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cinema.DataProcessor.ExportDto
{
    public class ExportMovieDto
    {
        [JsonProperty("MovieName")]
        public string Name { get; set; }

        public string Rating { get; set; }

        public string TotalIncomes { get; set; }

        public ExportCustomerDto[] Customers { get; set; }
    }
}
