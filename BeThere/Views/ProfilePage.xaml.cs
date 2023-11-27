using BeThere.ViewModels;

namespace BeThere.Views;

public partial class ProfilePage : ContentPage
{
    private readonly ProfileViewModle r_ProfileViewModle;
    public ProfilePage(ProfileViewModle i_ProfileViewModle)
	{
		InitializeComponent();
		BindingContext = i_ProfileViewModle;
        r_ProfileViewModle = i_ProfileViewModle;
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        r_ProfileViewModle.updateUserDetails();
    }
}
