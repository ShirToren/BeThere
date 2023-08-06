using BeThere.ViewModels;

namespace BeThere.Views;

public partial class ProfilePage : ContentPage
{
	public ProfilePage(ProfileViewModle i_ProfileViewModle)
	{
		InitializeComponent();
		BindingContext = i_ProfileViewModle;

    }
}
