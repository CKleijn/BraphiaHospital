using FluentValidation;

namespace Consultancy.Features.ConsultFeature.UpdateQuestion.Command
{
    public sealed class UpdateQuestionCommandValidator
        : AbstractValidator<UpdateQuestionCommmand>
    {
        public UpdateQuestionCommandValidator()
        {
            RuleFor(question => question.AnswerValue)
                .NotEmpty()
                .WithMessage("Answer is required!");
        }
    }
}
