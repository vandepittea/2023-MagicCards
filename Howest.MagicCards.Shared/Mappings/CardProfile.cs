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

            CreateMap<CardInDeckDto, CardInDeck>()
                .ForMember(dest => dest._id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Count));

            CreateMap<CardInDeck, CardInDeckDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src._id))
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Count));
        }
    }
}
