using System;
namespace BeThere.Models
{
	public class QuestionAnswers
	{
        public string questionId { get; set; } = null!;
        public List<UserAnswer> userAnswer { get; set; } = null!;
    }
}

