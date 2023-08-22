using Mopups.Services;
using BeThere.Models;
using BeThere.Services;

namespace BeThere.Views;


public partial class PopupPage
{
    private QuestionToAsk m_Question;
    private AnswerService m_AnswerService;
    public PopupPage(QuestionToAsk i_Question, AnswerService i_AnswerService)
    {
        InitializeComponent();
        m_Question = i_Question;
        m_AnswerService =  i_AnswerService;
        QuestionLabel.Text = m_Question.Question;
    }
    private void CloseButton_Clicked(object sender, EventArgs e)
    {
        MopupService.Instance.PopAsync();
    }
    private async void AnswerButton_Clicked(object sender, EventArgs e)
    {
        //await m_ChatService.JoinChatRoom(m_QuestionsList.ChatRoomId);
        //await GoToChatPage();
        Guid guid = Guid.NewGuid();
        string uuidString = guid.ToString();
        UserAnswer newAnswer = new UserAnswer(LogedInUser.LogedInUserName(), AnswerText.Text, m_Question.QuestionId, uuidString, DateTime.Now);
        await m_AnswerService.TryPostNewAnswer(newAnswer);
        await MopupService.Instance.PopAsync();
    }

    public async Task GoToChatPage()
    {
        var navigationParameter = new Dictionary<string, object>
        {
            ["ChatRoomId"] = m_Question.ChatRoomId
        };
        await Shell.Current.GoToAsync($"{nameof(ChatPage)}", navigationParameter);
       // await Shell.Current.GoToAsync(nameof(ChatPage));
    }
}