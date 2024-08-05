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
        Dim menu As New List(Of String) From
            {
                Choices.Cancel
            }
        If item.CanUse Then
            menu.Add(Choices.Use)
        End If
        Select Case ui.Choose((Mood.Prompt, String.Empty), menu.ToArray)
            Case Choices.Cancel
                Return endState
            Case Choices.Use
                item.Use(model.State.Avatar.State.Actor)
                Return New MessageState(model, ui, endState)
            Case Else
                Throw New NotImplementedException
        End Select
    End Function
End Class
