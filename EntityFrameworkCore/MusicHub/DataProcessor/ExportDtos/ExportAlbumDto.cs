using System.Collections;
using System.Collections.Generic;

namespace MusicHub.DataProcessor.ExportDtos
{
    public class ExportAlbumDto
    {
        public string AlbumName { get; set; }

        public string ReleaseDate { get; set; }

        public string ProducerName { get; set; }

        public ICollection<ExportSongDto> Songs { get; set; } = new HashSet<ExportSongDto>();

        public string AlbumPrice { get; set; }
    }
}
