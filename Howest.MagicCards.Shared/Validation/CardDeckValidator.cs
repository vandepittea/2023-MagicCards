namespace Howest.MagicCards.Shared.Validation
{
    public class CardDeckValidator : AbstractValidator<CardInDeckDto>
    {
        public CardDeckValidator()
        {
            RuleFor(card => card.Id)
                .NotEmpty().WithMessage("Id is required.")
                .Must(i => int.TryParse(i, out _)).WithMessage("Id must be a valid integer.")
                .Must(BeGreaterThanZero).WithMessage("Id must be greater than 0.");

            RuleFor(card => card.Count)
                .NotEmpty().WithMessage("Count is required.")
                .Must(c => int.TryParse(c, out _)).WithMessage("Count must be a valid integer.")
                .Must(BeGreaterThanOrEqualToOne).WithMessage("Count must be 1 or higher.");
        }

        private bool BeGreaterThanZero(string value)
        {
            if (int.TryParse(value, out int count))
            {
                return count > 0;
            }
            return false;
        }

        private bool BeGreaterThanOrEqualToOne(string value)
        {
            if (int.TryParse(value, out int count))
            {
                return count >= 1;
            }
            return false;
        }
    }
}