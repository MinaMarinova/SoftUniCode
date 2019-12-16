using System.Collections.Generic;

namespace TeisterMask.DataProcessor.ExportDto
{
    public class ExportEmployeeDto
    {
        public string Username { get; set; }

        public List<ExportTaskDto> Tasks { get; set; }
    }
}
