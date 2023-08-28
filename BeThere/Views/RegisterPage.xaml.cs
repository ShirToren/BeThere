using BeThere.ViewModels;

namespace BeThere.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegisterViewModel i_RegisterLogic)
    {
        InitializeComponent();
        BindingContext = i_RegisterLogic;
    }

    //protected override bool OnBackButtonPressed()
    //{
    //    Shell.Current.GoToAsync($"{nameof(LoginPage)}");
    //    return true;
    //}
    private void BirthdateDatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        DateTime birthdate = e.NewDate;
        DateTime today = DateTime.Today;
        int age = today.Year - birthdate.Year;
        if (birthdate > today.AddYears(-age))
            age--;

        // Update ViewModel's Age property
        if (BindingContext is RegisterViewModel viewModel)
        {
            viewModel.User.Age = age;
        }
    }
}