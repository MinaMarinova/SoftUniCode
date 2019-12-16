namespace SoftJail
{
    using AutoMapper;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ExportDto;
    using SoftJail.DataProcessor.ImportDto;
    using System;

    public class SoftJailProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public SoftJailProfile()
        {
            this.CreateMap<ImportOfficerDto, Officer>()
                .ForMember(x => x.Position, y => y.MapFrom(s => (Position)Enum.Parse(typeof(Position), s.Position)))
                .ForMember(x => x.Weapon, y => y.MapFrom(s => (Weapon)Enum.Parse(typeof(Weapon), s.Weapon)));

            this.CreateMap<Mail, ExportMailDto>()
                .ForMember(x => x.Description, y => y.MapFrom(s => Reverse(s.Description)));

            this.CreateMap<Prisoner, ExportPrisonerWithMailsDto>()
                .ForMember(x => x.IncarcerationDate, y => y.MapFrom(s => s.IncarcerationDate.ToString("yyyy-MM-dd")));
        }

        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
