Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class ItemStackInspectState
    Inherits BaseState
    Implements IState

    Private ReadOnly itemStack As IAvatarInventoryItemStackModel

    Public Sub New(model As IUniverseModel, ui As IUIContext, endState As IState,
                  itemStack As IAvatarInventoryItemStackModel)
        MyBase.New(model, ui, endState)
        Me.itemStack = itemStack
    End Sub

    Public Overrides Function Run() As IState
        ui.Clear()
        ui.WriteLine((Mood.Title, itemStack.ItemTypeName))
        ui.WriteLine((Mood.Info, $"Count: {itemStack.Count}"))
        Dim menu As New List(Of (String, IAvatarInventoryItemSubstackModel)) From
            {
                (Choices.Cancel, Nothing)
            }
        menu.AddRange(itemStack.Substacks.Select(Function(x) (x.EntityName, x)))
        Dim choice = ui.Choose((Mood.Prompt, String.Empty), menu.ToArray)
        If choice IsNot Nothing Then
            If choice.Items.Count = 1 Then
                Return New ItemInspectState(model, ui, Me, choice.Items.Single)
            End If
            Return New ItemSubstackInspectState(model, ui, Me, choice)
        End If
        Return New InventoryActionSelectState(model, ui, endState, itemStack)
    End Function
End Class
