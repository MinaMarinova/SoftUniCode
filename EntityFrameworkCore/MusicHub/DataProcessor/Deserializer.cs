namespace MusicHub.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using AutoMapper;
    using Data;
    using MusicHub.Data.Models;
    using MusicHub.Data.Models.Enums;
    using MusicHub.DataProcessor.ImportDtos;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data";

        private const string SuccessfullyImportedWriter
            = "Imported {0}";
        private const string SuccessfullyImportedProducerWithPhone
            = "Imported {0} with phone: {1} produces {2} albums";
        private const string SuccessfullyImportedProducerWithNoPhone
            = "Imported {0} with no phone number produces {1} albums";
        private const string SuccessfullyImportedSong
            = "Imported {0} ({1} genre) with duration {2}";
        private const string SuccessfullyImportedPerformer
            = "Imported {0} ({1} songs)";

        public static string ImportWriters(MusicHubDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var writersDtos = JsonConvert.DeserializeObject<WriterDto[]>(jsonString);

            foreach (var writerDto in writersDtos)
            {
                if (!IsValid(writerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                else
                {
                    var writer = new Writer
                    {
                        Name = writerDto.Name,
                        Pseudonym = writerDto.Pseudonym

                    };
                    context.Writers.Add(writer);
                    context.SaveChanges();

                    sb.AppendLine(String.Format(SuccessfullyImportedWriter, writer.Name));
                }
            }
            return sb.ToString().TrimEnd();
        }

        public static string ImportProducersAlbums(MusicHubDbContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var settings = new JsonSerializerSettings()
            {
                DateFormatString = "dd/MM/yyyy"
            };

            var producersDto = JsonConvert.DeserializeObject<ProducerDto[]>(jsonString, settings);

            foreach (var producerDto in producersDto)
            {
                if (!IsValid(producerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool areAllAlbumsValid = true;

                foreach (var album in producerDto.Albums)
                {
                    if (!IsValid(album))
                    {
                        sb.AppendLine(ErrorMessage);
                        areAllAlbumsValid = false;
                        break;
                    }
                }

                if (areAllAlbumsValid)
                {
                    var producer = new Producer
                    {
                        Name = producerDto.Name,
                        Pseudonym = producerDto.Pseudonym,
                        PhoneNumber = producerDto.PhoneNumber
                    };

                    foreach (var albumDto in producerDto.Albums)
                    {
                        var album = new Album
                        {
                            Name = albumDto.Name,
                            ReleaseDate = albumDto.ReleaseDate
                        };
                        producer.Albums.Add(album);
                    }

                    context.Albums.AddRange(producer.Albums);
                    context.Producers.Add(producer);
                    context.SaveChanges();

                    if (producer.PhoneNumber != null)
                    {
                        sb.AppendLine(String.Format(SuccessfullyImportedProducerWithPhone, producer.Name, producer.PhoneNumber, producer.Albums.Count));
                    }
                    else
                    {
                        sb.AppendLine(String.Format(SuccessfullyImportedProducerWithNoPhone, producer.Name, producer.Albums.Count));
                    }
                }

            }
            return sb.ToString().TrimEnd();
        }


        public static string ImportSongs(MusicHubDbContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            SongDto[] songDtos;

            using (var reader = new StringReader(xmlString))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SongDto[]),
                    new XmlRootAttribute("Songs"));
                
                songDtos = (SongDto[])serializer.Deserialize(reader);
            }
            
            foreach (var songDto in songDtos)
            {
                var isValidEnum = Enum.TryParse<Genre>(songDto.Genre, out Genre genre);
                
                if (!IsValid(songDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!isValidEnum)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!context.Writers.Any(w => w.Id == songDto.WriterId))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!context.Albums.Any(a => a.Id == songDto.AlbumId))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                
                var song = Mapper.Map<Song>(songDto);

                context.Songs.Add(song);

                context.SaveChanges();

                sb.AppendLine(String.Format(SuccessfullyImportedSong, song.Name, song.Genre, song.Duration));
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportSongPerformers(MusicHubDbContext context, string xmlString)
        {
            PerformerDto[] performerDtos;

            StringBuilder sb = new StringBuilder();

            using (var reader = new StringReader(xmlString))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(PerformerDto[]),
                    new XmlRootAttribute("Performers"));

                performerDtos = (PerformerDto[])serializer.Deserialize(reader);
            }

            foreach (var performerDto in performerDtos)
            {
                if (!IsValid(performerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool areAllSongsValid = true;

                foreach (var song in performerDto.PerformersSongs)
                {
                    if (!context.Songs.Any(s => s.Id == song.Id))
                    {
                        sb.AppendLine(ErrorMessage);
                        areAllSongsValid = false;
                        break;
                    }
                }

                if (areAllSongsValid)
                {
                    var performer = Mapper.Map<Performer>(performerDto);

                    var songsPerformers = new List<SongPerformer>();

                    foreach (var songForPerformer in performerDto.PerformersSongs)
                    {
                        var songPerformer = new SongPerformer
                        {
                            SongId = songForPerformer.Id,
                            Performer = performer
                        };

                        performer.PerformerSongs.Add(songPerformer);
                        songsPerformers.Add(songPerformer);
                    }

                    sb.AppendLine(String.Format(SuccessfullyImportedPerformer, performer.FirstName, performer.PerformerSongs.Count));

                    context.Performers.Add(performer);
                    context.SongsPerformers.AddRange(songsPerformers);
                    context.SaveChanges();
                }
            }

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object entity)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(entity);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(entity, validationContext, validationResult, true);

            return isValid;
        }
    }
}