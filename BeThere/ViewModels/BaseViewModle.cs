using BeThere;
using BeThere.Models;
using BeThere.Services;
using BeThere.ViewModels;
using BeThere.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using Plugin.LocalNotification;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;


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

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }
    }
}

