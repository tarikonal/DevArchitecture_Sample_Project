
using Business.Handlers.Games.Commands;
using FluentValidation;

namespace Business.Handlers.Games.ValidationRules
{

    public class CreateGameValidator : AbstractValidator<CreateGameCommand>
    {
        public CreateGameValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Price).NotEmpty();

        }
    }
    public class UpdateGameValidator : AbstractValidator<UpdateGameCommand>
    {
        public UpdateGameValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Price).NotEmpty();

        }
    }
}