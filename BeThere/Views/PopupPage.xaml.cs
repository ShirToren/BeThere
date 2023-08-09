using Mopups.Services;

namespace BeThere.Views;


public partial class PopupPage
{
    public PopupPage()
    {
        InitializeComponent();
    }

    private void LoginButton_Clicked(object sender, EventArgs e)
    {
        MopupService.Instance.PopAsync();
        //nevigate to chat
    }
}