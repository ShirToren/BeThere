using BeThere.Models;
using BeThere.ViewModels;
namespace BeThere.Views;

public partial class DetailsQuestionPage : ContentPage
{
    private DetailsQuestionViewModle m_DetailsViewModle;
    public DetailsQuestionPage(DetailsQuestionViewModle i_DetailsViewModle)
    {
        InitializeComponent();
        m_DetailsViewModle = i_DetailsViewModle;
        BindingContext = i_DetailsViewModle;

    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        m_DetailsViewModle.UpdateAnswersList();
    }
}
