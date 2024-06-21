using Consultancy.Common.Entities;
using Consultancy.Common.Entities.DTO;

namespace Consultancy.Common.Interfaces
{
    public interface IQuestionMapper
    {
        ICollection<Question> QuestionsDTOToQuestions(ICollection<QuestionDTO> QuestionsDTO);
    }
}
