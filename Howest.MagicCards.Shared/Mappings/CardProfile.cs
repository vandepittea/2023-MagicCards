namespace Howest.MagicCards.Shared.Mappings
{
    public class CardProfile : Profile
    {
        public CardProfile()
        {
            CreateMap<Card, CardDto>()
                .ForMember(dto => dto.CardTypes,
                           opt => opt.MapFrom(c => c.CardTypes.Select(ct => ct.Type.Name)))
                .ForMember(dto => dto.SetName,
                           opt => opt.MapFrom(c => c.SetCodeNavigation.Name))
                .ForMember(dto => dto.RarityName,
                           opt => opt.MapFrom(c => c.RarityCodeNavigation.Name))
                .ForMember(dto => dto.ArtistName,
                           opt => opt.MapFrom(c => c.Artist.FullName))
                .ReverseMap();

            CreateMap<Card, CardDetailDto>()
                .IncludeBase<Card, CardDto>()
                .ForMember(dto => dto.CardColors,
                           opt => opt.MapFrom(c => c.CardColors.Select(cc => cc.Color.Name)))
                .ReverseMap();

            CreateMap<CardInDeckDto, CardInDeck>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Count));

            CreateMap<CardInDeck, CardInDeckDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Count));

            CreateMap<CardDto, CardInDeckDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => 1));

            CreateMap<DAL.Models.Type, string>()
            .ConvertUsing(source => source.Name);
        }
    }
}
