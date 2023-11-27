using BeThere.ViewModels;
namespace BeThere.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(ConnectToAppViewModle i_LoginLogic)
    {
        InitializeComponent();
        BindingContext = i_LoginLogic;
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        PasswordEntry.Text = String.Empty;
        userNameEntry.Text = String.Empty;
    }
}