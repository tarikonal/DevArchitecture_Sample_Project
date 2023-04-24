
using Business.Handlers.Gamers.Commands;
using FluentValidation;

namespace Business.Handlers.Gamers.ValidationRules
{

    public class CreateGamerValidator : AbstractValidator<CreateGamerCommand>
    {
        public CreateGamerValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.BirthYear).NotEmpty();
            RuleFor(x => x.IdentityNumber).NotEmpty();

        }
    }
    public class UpdateGamerValidator : AbstractValidator<UpdateGamerCommand>
    {
        public UpdateGamerValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.BirthYear).NotEmpty();
            RuleFor(x => x.IdentityNumber).NotEmpty();

        }
    }
}