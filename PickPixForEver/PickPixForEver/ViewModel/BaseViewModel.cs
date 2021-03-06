﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PickPixForEver.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public List<string> PrivacyList { get; private set; } = new List<string> { "Public", "Private" };
        public List<string> TagTypeList { get; private set; } = new List<string> { "People", "Places", "Events", "Custom", "Relationship" };

        public Command LoadItemCommand { get; set; }
        public Command SearchItemComand { get; set; }
        public Command AddItemCommand { get; set; }
        public bool IsLoggedIn { get; set; }
        public string DisplayName { get; set; }
        public BaseViewModel()
        {
            if (!string.IsNullOrEmpty(Preferences.Get("email", "")))
            {
                IsLoggedIn = true;
                DisplayName = Preferences.Get("fullName", "");
            }
            else
            {
                IsLoggedIn = false;
                DisplayName = "Guest";
            }
        }

       

        bool isBusy;
        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                SetProperty(ref isBusy, value);
            }
        }

        string title = string.Empty;
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                SetProperty(ref title, value); 
            }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
           [CallerMemberName]string propertyName = "",
           Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
      }
}
