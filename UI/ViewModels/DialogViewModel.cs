using CommunityToolkit.Mvvm.Input;
using System;

namespace EstragoniaTemplate.UI.ViewModels;

public partial class DialogViewModel : ViewModel
{
    public string Message { get; private set; } = "";
    public string? CancelText { get; private init; }
    public string? DenyText { get; private init; }
    public string? ConfirmText { get; private init; }

    public enum Response
    {
        Cancel = 0,
        Deny = 1,
        Confirm = 2
    }

    public event Action<Response>? Responded;

    public DialogViewModel() { }
    public DialogViewModel(string message, string? cancelText = null, string? denyText = null, string? confirmText = null)
    {
        Message = message;
        CancelText = cancelText;
        DenyText = denyText;
        ConfirmText = confirmText;
    }

    [RelayCommand]
    public void ButtonResponse(int type)
    {
        Responded?.Invoke((Response)type);
        Close();
    }
}
