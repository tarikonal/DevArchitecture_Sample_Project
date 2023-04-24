
using Business.Handlers.Campaigns.Commands;
using FluentValidation;

namespace Business.Handlers.Campaigns.ValidationRules
{

    public class CreateCampaignValidator : AbstractValidator<CreateCampaignCommand>
    {
        public CreateCampaignValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.StartDate).NotEmpty();
            RuleFor(x => x.EndDate).NotEmpty();
            RuleFor(x => x.DiscountRate).NotEmpty();
            RuleFor(x => x.GameId).NotEmpty();

        }
    }
    public class UpdateCampaignValidator : AbstractValidator<UpdateCampaignCommand>
    {
        public UpdateCampaignValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.StartDate).NotEmpty();
            RuleFor(x => x.EndDate).NotEmpty();
            RuleFor(x => x.DiscountRate).NotEmpty();
            RuleFor(x => x.GameId).NotEmpty();

        }
    }
}