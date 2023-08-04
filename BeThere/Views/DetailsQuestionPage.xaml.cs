using BeThere.ViewModels;
namespace BeThere.Views;

public partial class DetailsQuestionPage : ContentPage
{
    public DetailsQuestionPage(DetailsQuestionViewModle i_DetailsViewModle)
    {
        InitializeComponent();
        BindingContext = i_DetailsViewModle;

    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}
