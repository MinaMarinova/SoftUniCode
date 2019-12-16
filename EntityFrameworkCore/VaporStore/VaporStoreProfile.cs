namespace VaporStore
{
	using AutoMapper;
    using System;
    using System.Globalization;
    using VaporStore.Data;
    using VaporStore.Data.Models;
    using VaporStore.DataProcessor.ImportDtos;

    public class VaporStoreProfile : Profile
	{
		// Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
		public VaporStoreProfile()
		{

            this.CreateMap<ImportPurchaseDto, Purchase>()
                .ForMember(x => x.Date, y => y.MapFrom
                        (s => DateTime.ParseExact(s.Date, @"dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)));
                
		}
	}
}