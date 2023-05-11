using AutoMapper;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared
{
    public class CardProfile : Profile
    {
        public CardProfile()
        {
            CreateMap<Card, CardDto>()
                .ForMember(dto => dto.CardColors,
                           opt => opt.MapFrom(c => c.CardColors.Select(cc => cc.Color.Code)))
                .ForMember(dto => dto.CardTypes,
                           opt => opt.MapFrom(c => c.CardTypes.Select(ct => ct.Type)))
                .ReverseMap();

            CreateMap<Card, CardDetailDto>()
                .IncludeBase<Card, CardDto>()
                .ForMember(dto => dto.ArtistName,
                           opt => opt.MapFrom(c => c.Artist.FullName))
                .ReverseMap();
        }
    }
}
