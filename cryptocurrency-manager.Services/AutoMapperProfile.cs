using AutoMapper;
using cryptocurrency_manager.DataContext.Dtos;
using DataContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cryptocurrency_manager.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Crypto mapping
            CreateMap<Cryptocurrency, CryptoDto>().ReverseMap();
            CreateMap<CryptoCreateDto, Cryptocurrency>();
            CreateMap<Cryptocurrency, CryptoHistoryDto>();
            //.ForMember(dest => dest.History, opt => opt.MapFrom(src => src.History));


            // History mapping
            CreateMap<History, HistoryDto>();

        }
    }
}
