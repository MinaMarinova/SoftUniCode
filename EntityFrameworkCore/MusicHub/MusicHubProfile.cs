namespace MusicHub
{
    using AutoMapper;
    using MusicHub.Data.Models;
    using MusicHub.DataProcessor.ExportDtos;
    using MusicHub.DataProcessor.ImportDtos;
    using System;
    using System.Globalization;
    using System.Linq;

    public class MusicHubProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public MusicHubProfile()
        {
            this.CreateMap<SongDto, Song>()
                .ForMember(x => x.Duration, y => y.MapFrom(s => TimeSpan.Parse(s.Duration)))
                .ForMember(x => x.CreatedOn, y => y.MapFrom(s => DateTime.ParseExact(s.CreatedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture)));

            this.CreateMap<PerformerDto, Performer>();

            this.CreateMap<Song, ExportSongWithDurationDto>()
                .ForMember(x => x.PerformerName, y => y.MapFrom(s => s.SongPerformers
                                                    .Select(sp => sp.Performer.FirstName + " " + sp.Performer.LastName)
                                                    .FirstOrDefault()))
                .ForMember(x => x.AlbumProducer, y => y.MapFrom(s => s.Album.Producer.Name))
                .ForMember(x => x.Duration, y => y.MapFrom(s => s.Duration.ToString(@"hh\:mm\:ss")));
        }
    }
}
