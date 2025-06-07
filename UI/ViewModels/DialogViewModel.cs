using CommunityToolkit.Mvvm.Input;
using EstragoniaTemplate.Main;
using System;

namespace EstragoniaTemplate.UI.ViewModels;

public partial class DialogViewModel : ViewModel
{
    public string Message { get; private set; } = "";
    public string? CancelText { get; }
    public string? DenyText { get; }
    public string? ConfirmText { get; }

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

    public static void OpenDialog(UserInterface dialogUserInterface, FocusStack focusStack, DialogViewModel dialog, Action<Response> onResponse)
    {
        dialog.Responded += OnResponse;

        void OnResponse(Response response)
        {
            dialog.Responded -= OnResponse;
            onResponse(response);
        }

        dialogUserInterface.MainViewModel.NavigateTo(dialog);
        focusStack.Push(dialogUserInterface);
        dialog.Closed += (_) => focusStack.Pop();
    }

    [RelayCommand]
    public void ButtonResponse(int type)
    {
        Responded?.Invoke((Response)type);
        Close();
    }
}
