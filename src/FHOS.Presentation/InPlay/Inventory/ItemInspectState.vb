Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class ItemInspectState
    Inherits BaseState
    Implements IState

    Private ReadOnly item As IItemModel

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState, item As IItemModel)
        MyBase.New(model, ui, endState)
        Me.item = item
    End Sub

    Public Overrides Function Run() As IState
        ui.Clear()
        ui.WriteLine(
            (Mood.Prompt, item.UniqueName),
            (Mood.Info, $"Description: {item.Description}"))
        Dim menu As New List(Of (String, IAvatarItemDialogModel)) From
            {
                (Choices.Cancel, Nothing)
            }
        menu.AddRange(model.State.Avatar.Dialogs(item))
        Dim choice = ui.Choose((Mood.Prompt, String.Empty), menu.ToArray)
        If choice Is Nothing Then
            Return endState
        End If
        choice.Start()
        Return New DialogState(model, ui, endState)
    End Function
End Class
