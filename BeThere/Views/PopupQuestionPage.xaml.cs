using Mopups.Services;
using BeThere.Models;
using BeThere.Services;

namespace BeThere.Views;


public partial class PopupQuestionPage
{
    private QuestionToAsk m_Question;
    private AnswerService m_AnswerService;
    private ChatService m_ChatService;
    private CreditsService m_CreditsService;
    public PopupQuestionPage(QuestionToAsk i_Question, AnswerService i_AnswerService, ChatService i_ChatService, CreditsService i_CreditsService)
    {
        InitializeComponent();
        m_Question = i_Question;
        m_AnswerService =  i_AnswerService;
        m_ChatService = i_ChatService;
        m_CreditsService = i_CreditsService;
        DescriptionLabel.Text = m_Question.Question;
        Title.Text = "New Question from " + i_Question.UserName;
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
            await m_CreditsService.TryUpdateCredits(5);
            LogedInUser.LogedInUserObject().Credits += 5;
            //await m_ChatService.JoinChatRoom(newAnswer.ChatRoomId);
            await m_ChatService.CreateChatRoom(newAnswer.ChatRoomId);
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