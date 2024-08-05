Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class ItemSubstackInspectState
    Inherits BaseState
    Implements IState

    Private ReadOnly substack As IAvatarInventoryItemSubstackModel

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState, substack As IAvatarInventoryItemSubstackModel)
        MyBase.New(model, ui, endState)
        Me.substack = substack
    End Sub

    Public Overrides Function Run() As IState
        ui.Clear()
        ui.WriteLine((Mood.Prompt, substack.EntityName))
        Dim menu As New List(Of (Text As String, Value As IItemModel)) From
            {
                (Choices.Cancel, Nothing)
            }
        menu.AddRange(substack.Items.Select(Function(x) (x.UniqueName, x)))
        Dim choice = ui.Choose((Mood.Prompt, String.Empty), menu.ToArray)
        If choice Is Nothing Then
            Return endState
        End If
        Return New ItemInspectState(model, ui, Me, choice)
    End Function
End Class
