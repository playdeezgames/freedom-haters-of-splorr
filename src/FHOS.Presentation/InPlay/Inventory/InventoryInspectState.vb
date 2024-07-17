Imports FHOS.Model
Imports SPLORR.Presentation

Friend Class InventoryInspectState
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
        ui.WriteLine((Mood.Title, itemStack.ItemName))
        ui.WriteLine((Mood.Info, $"Count: {itemStack.Count}"))
        ui.Message((Mood.Prompt, String.Empty))
        Return New InventoryActionSelectState(model, ui, endState, itemStack)
    End Function
End Class
