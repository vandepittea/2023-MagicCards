using FluentValidation;
using Howest.MagicCards.Shared.DTOs;

namespace Howest.MagicCards.Shared
{
    public class CardValidator : AbstractValidator<CardDto>
    {
        public CardValidator()
        {
            RuleFor(c => c.Name).NotEmpty().MaximumLength(50);
            RuleFor(c => c.Set).NotEmpty().MaximumLength(10);
            RuleFor(c => c.ArtistName).NotEmpty();
            RuleFor(c => c.Type).NotEmpty().MaximumLength(20);
            RuleFor(c => c.Rarity).NotEmpty().MaximumLength(10);
            RuleFor(c => c.ManaCost).MaximumLength(20);
            RuleFor(c => c.Text).MaximumLength(2000);
            RuleFor(c => c.Flavor).MaximumLength(2000);
            RuleFor(c => c.Power).MaximumLength(10);
            RuleFor(c => c.Toughness).MaximumLength(10);
            RuleFor(c => c.Number).MaximumLength(20);
            RuleFor(c => c.Layout).NotEmpty().MaximumLength(20);
            RuleFor(c => c.OriginalImageUrl).MaximumLength(255);
            RuleFor(c => c.OriginalText).MaximumLength(2000);
            RuleFor(c => c.OriginalType).MaximumLength(50);
            RuleFor(c => c.Image).MaximumLength(255);
            RuleFor(c => c.MtgId).MaximumLength(20);
        }
    }
}