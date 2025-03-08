using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Animation;
using EstragoniaTemplate.UI.Controls;
using System;
using EstragoniaTemplate.Main;

namespace EstragoniaTemplate.UI.ViewModels;

public partial class OptionsControlsViewModel : ViewModel, IOptionsTabViewModel
{
    public void TryClose(Action callOnClose) => callOnClose();
}
