using BeThere.Models;
using BeThere.ViewModels;
namespace BeThere;

public partial class MainPage : ContentPage
{
    private HomeViewModle m_HomeViewModel;
    public MainPage(HomeViewModle i_HomeViewModle)
    {
        InitializeComponent();
        BindingContext = i_HomeViewModle;
        m_HomeViewModel = i_HomeViewModle;
    }
}