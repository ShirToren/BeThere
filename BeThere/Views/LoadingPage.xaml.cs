using BeThere.Services;
namespace BeThere.Views;

public partial class LoadingPage : ContentPage
{
    private AuthonticationService m_AuthService;

	public LoadingPage(AuthonticationService i_AuthService)
	{
        InitializeComponent();
        m_AuthService = i_AuthService;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {

        base.OnNavigatedTo(args);
        bool isAuthenticated = await m_AuthService.isAuthenticatedAsync();
        if (isAuthenticated == true)
        {
            await m_AuthService.GetAuthLoginUser();
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }
        else
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
   
    }
}
