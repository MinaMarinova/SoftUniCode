using AutoMapper;
using Cinema.Data.Models;
using Cinema.DataProcessor.ExportDto;
using Cinema.DataProcessor.ImportDto;
using System;
using System.Globalization;
using System.Linq;

namespace Cinema
{
    public class CinemaProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public CinemaProfile()
        {
            this.CreateMap<ImportMovieDto, Movie>()
                .ForMember(x => x.Duration, y => y.MapFrom(s => TimeSpan.Parse(s.Duration)));

            this.CreateMap<ImportProjectionDto, Projection>()
                .ForMember(x => x.DateTime, y => y.MapFrom(s => DateTime.ParseExact(s.DateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));

            this.CreateMap<ImportTicketDto, Ticket>();

            this.CreateMap<ImportCustomerDto, Customer>();

            this.CreateMap<Customer, ExportCustomerDto>()
                .ForMember(x => x.Balance, y => y.MapFrom(s => s.Balance.ToString("f2")));

            this.CreateMap<Movie, ExportMovieDto>()
                .ForMember(x => x.Rating, y => y.MapFrom(s => s.Rating.ToString("f2")))
                .ForMember(x => x.TotalIncomes, y => y.MapFrom(s => s.Projections.Select(p => p.Tickets.Sum(t => t.Price).ToString("f2"))));

            this.CreateMap<Customer, ExportCustomerDurationDto>()
                .ForMember(x => x.SpentMoney, y => y.MapFrom(s => s.Tickets.Sum(t => t.Price).ToString("f2")))
                .ForMember(x => x.SpentTime, y => y.MapFrom(s => new TimeSpan(s.Tickets.Sum(t => t.Projection.Movie.Duration.Ticks)).ToString(@"hh\:mm\:ss")));
        }


    }
}
