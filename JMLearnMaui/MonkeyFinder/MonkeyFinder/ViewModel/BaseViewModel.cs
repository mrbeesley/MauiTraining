﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyFinder.ViewModel;

#region Old School Binding Code

/*
internal class BaseViewModel : INotifyPropertyChanged
{
    bool isBusy;
    string title;

    public bool IsBusy
    {
        get => isBusy;
        set
        {
            if (isBusy != value)
                return;
            isBusy = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsNotBusy));
        }
    }

    public string Title
    {
        get => title;
        set
        {
            if (title == value)
                return;
            title = value;
            OnPropertyChanged();
        }
    }

    public bool IsNotBusy => !isBusy;

    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged([CallerMemberName]string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
*/

#endregion // Old School Binding Code

#region INotifyPropertyChanged attribute from mvvm community toolkit

/*
[INotifyPropertyChanged]
public partial class BaseViewModel
{
    bool isBusy;
    string title;

    //public bool IsNotBusy => !isBusy;

}
*/

#endregion // INotifyPropertyChanged attribute from mvvm community toolkit

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    bool isBusy;
    
    [ObservableProperty]
    string title;

    public bool IsNotBusy => !IsBusy;

}