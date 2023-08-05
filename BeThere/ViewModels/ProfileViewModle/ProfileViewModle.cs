using BeThere.Services;
using BeThere.Views;

namespace BeThere.ViewModels
{
	public class ProfileViewModle
	{
        private AuthonticationService m_AuthonticationService;

        public Command LogoutCommand { get; }

        public ProfileViewModle(AuthonticationService i_AuthService)
		{
            m_AuthonticationService = i_AuthService;
            LogoutCommand = new Command(async () => await logoutClicked());
        }

        private async Task logoutClicked()
        {
            m_AuthonticationService.Logout();
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

    }
}

