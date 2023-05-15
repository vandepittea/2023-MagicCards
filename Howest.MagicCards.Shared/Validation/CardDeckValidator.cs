namespace Howest.MagicCards.Shared.Validation
{
    public class CardDeckValidator : AbstractValidator<CardInDeckDto>
    {
        public CardDeckValidator()
        {
            RuleFor(card => card.Id).NotEmpty().WithMessage("Id is required.").GreaterThan(0).WithMessage("Id must be greater than 0.");
            RuleFor(card => card.Count).NotEmpty().WithMessage("Count is required.").GreaterThanOrEqualTo(1).WithMessage("Count must be 1 or higher.");
        }
    }
}