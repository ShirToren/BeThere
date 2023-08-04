using BeThere.ViewModels;
namespace BeThere.Views;

public partial class SetQuestionPage : ContentPage
{
	public SetQuestionPage(SetQestionToAskViewModle i_SetQuestion)
	{
		InitializeComponent();
		BindingContext = i_SetQuestion;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }


}
