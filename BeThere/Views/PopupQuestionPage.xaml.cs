using Mopups.Services;
using BeThere.Models;
using BeThere.Services;

namespace BeThere.Views;


public partial class PopupQuestionPage
{
    private QuestionToAsk m_Question;
    private AnswerService m_AnswerService;
    private ChatService m_ChatService;
    public PopupQuestionPage(QuestionToAsk i_Question, AnswerService i_AnswerService, ChatService i_ChatService)
    {
        InitializeComponent();
        m_Question = i_Question;
        m_AnswerService =  i_AnswerService;
        m_ChatService = i_ChatService;
        DescriptionLabel.Text = m_Question.Question;
    }
    private void CloseButton_Clicked(object sender, EventArgs e)
    {
        MopupService.Instance.PopAsync();
    }
    private async void AnswerButton_Clicked(object sender, EventArgs e)
    {
            Guid guid = Guid.NewGuid();
            string uuidString = guid.ToString();
            UserAnswer newAnswer = new UserAnswer(LogedInUser.LogedInUserName(), AnswerText.Text, m_Question.QuestionId, uuidString, DateTime.Now);
            await m_AnswerService.TryPostNewAnswer(newAnswer);
            await m_ChatService.JoinChatRoom(newAnswer.ChatRoomId);
            foreach (QuestionToAsk question in SharedDataSource.UsersPreviousQuestions)
            {
                if (question.QuestionId.Equals(newAnswer.QuestionId))
                {
                    question.NumOfAnswers++;
                }
            }
  
        await MopupService.Instance.PopAsync();
    }
}