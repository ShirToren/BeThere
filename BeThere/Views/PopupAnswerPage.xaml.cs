using BeThere.Models;
using BeThere.Services;
using Mopups.Services;

namespace BeThere.Views;

public partial class PopupAnswerPage
{
    private UserAnswer m_Answer;
    private AnswerService m_AnswerService;
    private ChatService m_ChatService;
    public PopupAnswerPage(UserAnswer i_Answer, AnswerService i_AnswerService, ChatService i_ChatService)
	{
		InitializeComponent();
        m_Answer = i_Answer;
        m_AnswerService = i_AnswerService;
        m_ChatService = i_ChatService;
        DescriptionLabel.Text = i_Answer.Text;
        Titel.Text = "New answer from " + i_Answer.Username;
        QuestionToAsk question = HistoryData.GetQuestionObjectByQuestionId(i_Answer.QuestionId);
        if (question != null)
        {
            QuestionLabel.Text = "For question: " + question.Question;
        }
    }

    private void CloseButton_Clicked(object sender, EventArgs e)
    {
        MopupService.Instance.PopAsync();
    }

    private async void StartChatButton_Clicked(object sender, EventArgs e)
    {
        await m_ChatService.JoinChatRoom(m_Answer.ChatRoomId);
        await GoToChatPage();

        await MopupService.Instance.PopAsync();
    }

    public async Task GoToChatPage()
    {
        var navigationParameter = new Dictionary<string, object>
        {
            ["ChatRoomId"] = m_Answer.ChatRoomId
        };
        await Shell.Current.GoToAsync($"{nameof(ChatPage)}", navigationParameter);
    }
}