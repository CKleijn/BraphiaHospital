using FluentValidation;

namespace Consultancy.Features.ConsultFeature.UpdateQuestions.Command
{
    public sealed class UpdateQuestionsCommandValidator
        : AbstractValidator<UpdateQuestionsCommmand>
    {
        public UpdateQuestionsCommandValidator()
        {
            RuleFor(consult => consult.Questions)
                .NotEmpty()
                .WithMessage("Questions are required!");
        }
    }
}
