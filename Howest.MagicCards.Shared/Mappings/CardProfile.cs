using AutoMapper;
using Howest.MagicCards.Shared.DTO.Howest.MagicCards.Shared.DTO;

namespace Howest.MagicCards.Shared.Mappings
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

            CreateMap<CardUpdateDto, Card>()
            .ForMember(dest => dest.Artist, opt => opt.MapFrom(src => new Artist { FullName = src.ArtistName }))
            .ForMember(dest => dest.RarityCodeNavigation, opt => opt.MapFrom(src => new Rarity { Code = src.RarityCode }))
            .ForMember(dest => dest.CardColors, opt => opt.MapFrom(src => src.CardColors.Select(cc => new CardColor { ColorId = cc })))
            .ForMember(dest => dest.CardTypes, opt => opt.MapFrom(src => src.CardTypes.Select(ct => new CardType { TypeId = ct })));

            CreateMap<CardWriteDto, Card>()
                .ForMember(dest => dest.Artist, opt => opt.MapFrom(src => new Artist { FullName = src.ArtistName }))
                .ForMember(dest => dest.RarityCodeNavigation, opt => opt.MapFrom(src => new Rarity { Code = src.RarityCode }))
                .ForMember(dest => dest.CardColors, opt => opt.MapFrom(src => src.CardColors.Select(cc => new CardColor { ColorId = cc })))
                .ForMember(dest => dest.CardTypes, opt => opt.MapFrom(src => src.CardTypes.Select(ct => new CardType { TypeId = ct })));
        }
    }
}
