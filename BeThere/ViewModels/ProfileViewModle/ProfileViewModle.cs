using BeThere.Models;
using BeThere.Services;
using BeThere.Views;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BeThere.ViewModels
{
	public partial class ProfileViewModle : BaseViewModel
	{
        private AuthonticationService m_AuthonticationService;

        public Command LogoutCommand { get; }

        [ObservableProperty]
        private string userName;
        [ObservableProperty]
        private string fullName;
        [ObservableProperty]
        private string email;
        [ObservableProperty]
        private string age;
        [ObservableProperty]
        private string credits;

        private UserData user => LogedInUser.LogedInUserObject();

        public ProfileViewModle(AuthonticationService i_AuthService)
		{
            m_AuthonticationService = i_AuthService;
            LogoutCommand = new Command(async () => await logoutClicked());
            UserData user = LogedInUser.LogedInUserObject();
            UserName = user.Username;
            FullName = user.FullName;
            Email = user.Email;
            Age = user.Age.ToString();
            Credits = user.Credits.ToString();
        }

        private async Task logoutClicked()
        {
            m_AuthonticationService.Logout();
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

        public void updateUserDetails()
        {
            UserData user = LogedInUser.LogedInUserObject();
            UserName = user.Username;
            FullName = user.FullName;
            Email = user.Email;
            Age = user.Age.ToString();
            Credits = user.Credits.ToString();
        }
    }
}

