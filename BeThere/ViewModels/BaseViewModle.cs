﻿using CommunityToolkit.Mvvm.ComponentModel;

namespace BeThere.ViewModels
{
    public partial class BaseViewModels : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool isBusy;

        [ObservableProperty]
        private string title;

        public bool IsNotBusy => !IsBusy;

    }
}
