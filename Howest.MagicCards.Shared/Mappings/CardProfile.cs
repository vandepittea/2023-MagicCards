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
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ManaCost, opt => opt.MapFrom(src => src.ManaCost))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.RarityCode, opt => opt.MapFrom(src => src.RarityCode))
            .ForMember(dest => dest.SetCode, opt => opt.MapFrom(src => src.SetCode))
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
            .ForMember(dest => dest.Flavor, opt => opt.MapFrom(src => src.Flavor))
            .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number))
            .ForMember(dest => dest.Power, opt => opt.MapFrom(src => src.Power))
            .ForMember(dest => dest.Toughness, opt => opt.MapFrom(src => src.Toughness))
            .ForMember(dest => dest.Layout, opt => opt.MapFrom(src => src.Layout))
            .ForMember(dest => dest.MultiverseId, opt => opt.MapFrom(src => src.MultiverseId))
            .ForMember(dest => dest.OriginalImageUrl, opt => opt.MapFrom(src => src.OriginalImageUrl))
            .ForMember(dest => dest.OriginalText, opt => opt.MapFrom(src => src.OriginalText))
            .ForMember(dest => dest.OriginalType, opt => opt.MapFrom(src => src.OriginalType))
            .ForMember(dest => dest.MtgId, opt => opt.MapFrom(src => src.MtgId))
            .ForMember(dest => dest.Variations, opt => opt.MapFrom(src => src.Variations))
            .ForMember(dest => dest.Artist, opt => opt.MapFrom(src => new Artist
            {
                FullName = src.ArtistName
            }))
            .ForMember(dest => dest.RarityCodeNavigation, opt => opt.MapFrom(src => new Rarity
            {
                Code = src.RarityCode
            }))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
            .ForMember(dest => dest.CardColors, opt => opt.MapFrom(src => src.CardColors.Select(cc => new CardColor
            {
                ColorCode = cc
            })))
            .ForMember(dest => dest.CardTypes, opt => opt.MapFrom(src => src.CardTypes.Select(ct => new CardType
            {
                TypeCode = ct
            })));
        }
    }
}
