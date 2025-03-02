using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Animation;
using EstragoniaTemplate.UI.Controls;
using System;
using static EstragoniaTemplate.UI.Utilities;

namespace EstragoniaTemplate.UI.ViewModels;

public partial class DialogViewModel : ViewModel
{
    public string Message { get; private set; } = "";
    public string CancelText { get; private init; } = "";
    public string DenyText { get; private init; } = "";
    public string ConfirmText { get; private init; } = "";

    public enum Response
    {
        Cancel = 0,
        Deny = 1,
        Confirm = 2
    }

    public event Action<Response>? UserResponded;

    public DialogViewModel() { }
    public DialogViewModel(string message, string cancelText, string denyText, string confirmText)
    {
        Message = message;
        CancelText = cancelText;
        DenyText = denyText;
        ConfirmText = confirmText;
    }

    [RelayCommand]
    public void ButtonResponse(int type)
    {
        UserResponded?.Invoke((Response)type);
        Close();
    }
}
