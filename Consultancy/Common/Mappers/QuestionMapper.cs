using Consultancy.Common.Entities;
using Consultancy.Common.Entities.DTO;
using Consultancy.Common.Interfaces;
using Riok.Mapperly.Abstractions;

namespace Consultancy.Common.Mappers
{
    [Mapper]
    public partial class QuestionMapper
        : IQuestionMapper
    {
        public ICollection<Question> QuestionsDTOToQuestions(ICollection<QuestionDTO> QuestionsDTO)
        {
            List<Question> result = [];

            foreach (var q in QuestionsDTO)
            {
                result.Add(new Question
                {
                    QuestionValue = q.QuestionValue,
                    AnswerValue = q.AnswerValue
                });
            }

            return result;
        }
    }
}
