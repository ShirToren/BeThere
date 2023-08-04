using BeThere.Models;
using BeThere.ViewModels;
namespace BeThere;

public partial class MainPage : ContentPage
{
    public MainPage(UsersHistoryViewModle i_HistoryViewModle)
    {
        InitializeComponent();
        BindingContext = i_HistoryViewModle;
    }
}