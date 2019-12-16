namespace MusicHub.DataProcessor
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using AutoMapper.QueryableExtensions;
    using Data;
    using MusicHub.DataProcessor.ExportDtos;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static object XMLSerializer { get; private set; }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context.Albums
                .Where(a => a.ProducerId == producerId)
                .OrderByDescending(a => context.Songs.Where(s => s.AlbumId == a.Id).Sum(sng => sng.Price))
                .Select(a => new ExportAlbumDto
                {
                    AlbumName = a.Name,
                    ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy"),
                    ProducerName = a.Producer.Name,
                    Songs = context.Songs.
                              OrderByDescending(s => s.Name)
                              .ThenBy(s => s.Writer.Name)
                               .Where(s => s.AlbumId == a.Id).Select(s => new ExportSongDto
                               {
                                   SongName = s.Name,
                                   Price = s.Price.ToString("f2"),
                                   Writer = s.Writer.Name
                               }).ToList(),
                    AlbumPrice = context.Songs.Where(s => s.AlbumId == a.Id).Sum(song => song.Price).ToString("f2")
                }).ToList();

            string jsonString = JsonConvert.SerializeObject(albums, Formatting.Indented);

            return jsonString;
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context.Songs
                .OrderBy(s => s.Name)
                .ThenBy(s => s.Writer.Name)
                .ThenBy(s => s.SongPerformers.Select(sp => sp.Performer.FirstName))
                .ThenBy(s => s.SongPerformers.Select(sp => sp.Performer.LastName))
                .Where(s => s.Duration.TotalSeconds > duration)
                .ProjectTo<ExportSongWithDurationDto>()
                .ToArray();

            StringBuilder sb = new StringBuilder();

            using (var writer = new StringWriter(sb))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ExportSongWithDurationDto[]),
                    new XmlRootAttribute("Songs"));

                var namespaces = new XmlSerializerNamespaces();
                namespaces.Add("", "");

                serializer.Serialize(writer, songs, namespaces);
            }

            return sb.ToString().TrimEnd();
              
        }
    }
}