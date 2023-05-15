using FluentValidation;

public class IntValidator : AbstractValidator<int>
{
    public IntValidator()
    {
        RuleFor(value => value).GreaterThan(0).WithMessage("Id must be greater than 0.");
    }
}
