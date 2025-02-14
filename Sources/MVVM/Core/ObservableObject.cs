﻿namespace StalcraftClanManager.MVVM.Core
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
