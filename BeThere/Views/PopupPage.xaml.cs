using Mopups.Services;
using BeThere.Models;

namespace BeThere.Views;


public partial class PopupPage
{
    private QuestionToAsk m_QuestionsList;
    public PopupPage(QuestionToAsk i_QuestionsList)
    {
        InitializeComponent();
        m_QuestionsList = i_QuestionsList;
        QuestionLabel.Text = m_QuestionsList.Question;
    }
    private void CloseButton_Clicked(object sender, EventArgs e)
    {
        MopupService.Instance.PopAsync();
    }
    private async void AnswerButton_Clicked(object sender, EventArgs e)
    {
        await GoToChatPage();
        await MopupService.Instance.PopAsync();
    }
    public async Task GoToChatPage()
    {
        await Shell.Current.GoToAsync(nameof(ChatPage));
    }
}