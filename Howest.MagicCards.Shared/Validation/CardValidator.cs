using FluentValidation;
using Howest.MagicCards.Shared.DTOs;

namespace Howest.MagicCards.Shared.Validation
{
    public class CardValidator : AbstractValidator<CardDto>
    {
        public CardValidator()
        {

        }
    }
}