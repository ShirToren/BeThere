using BeThere.Models;
using BeThere.ViewModels;

namespace BeThere;

public partial class MainPage : ContentPage
{
    private HomeViewModle m_HomeViewModel;
    private bool m_IsFirstAppearing = true;
    public MainPage(HomeViewModle i_HomeViewModle)
    {
        InitializeComponent();
        BindingContext = i_HomeViewModle;
        m_HomeViewModel = i_HomeViewModle;
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        m_HomeViewModel.HelloMessage = "Hello, " + LogedInUser.LogedInUserName();
    }
}