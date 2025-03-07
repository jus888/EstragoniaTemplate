using Avalonia.Animation;
using CommunityToolkit.Mvvm.ComponentModel;
using EstragoniaTemplate.Main;
using EstragoniaTemplate.UI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Godot;

namespace EstragoniaTemplate.UI.ViewModels;

public partial class MainViewModel : NavigatorViewModel
{
    public MainViewModel(UserInterface userInterface) : base(userInterface) { }
}
